using aplusautism.Bal.DTO;
using aplusautism.Bal.ILogic;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace aplusautism.Bal.Logic
{
    public class AzureStorage : IAzureStorage
    {
        #region Dependency Injection / Constructor

        private readonly string _storageConnectionString;
        private readonly string _storageContainerName;
        private readonly ILogger<AzureStorage> _logger;
        private readonly AppSettingsDTO _appSettings;

        public AzureStorage(IConfiguration configuration, IOptions<AppSettingsDTO> appSetting, ILogger<AzureStorage> logger)
        {
            //_storageConnectionString = _appSettings.BlobConnectionString;
            //_storageContainerName = _appSettings.BlobContainerName;
            _logger = logger;
            _appSettings = appSetting.Value;
        }

        #endregion

        public async Task<BlobResponseDto> DeleteAsync(string blobFilename)
        {
            BlobContainerClient client = new BlobContainerClient(_storageConnectionString, _storageContainerName);

            BlobClient file = client.GetBlobClient(blobFilename);

            try
            {
                // Delete the file
                await file.DeleteAsync();
            }
            catch (RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                // File did not exist, log to console and return new response to requesting method
                _logger.LogError($"File {blobFilename} was not found.");
                return new BlobResponseDto { Error = true, Status = $"File with name {blobFilename} not found." };
            }

            // Return a new BlobResponseDto to the requesting method
            return new BlobResponseDto { Error = false, Status = $"File: {blobFilename} has been successfully deleted." };

        }

        public async Task<BlobDto> DownloadAsync(string blobFilename)
        {
            // Get a reference to a container named in appsettings.json
            BlobContainerClient client = new BlobContainerClient(_storageConnectionString, _storageContainerName);

            try
            {
                // Get a reference to the blob uploaded earlier from the API in the container from configuration settings
                BlobClient file = client.GetBlobClient(blobFilename);

                // Check if the file exists in the container
                if (await file.ExistsAsync())
                {
                    var data = await file.OpenReadAsync();
                    Stream blobContent = data;

                    // Download the file details async
                    var content = await file.DownloadContentAsync();

                    // Add data to variables in order to return a BlobDto
                    string name = blobFilename;
                    string contentType = content.Value.Details.ContentType;

                    // Create new BlobDto with blob data from variables
                    return new BlobDto { Content = blobContent, Name = name, ContentType = contentType };
                }
            }
            catch (RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobNotFound)
            {
                // Log error to console
                _logger.LogError($"File {blobFilename} was not found.");
            }

            // File does not exist, return null and handle that in requesting method
            return null;
        }

        public async Task<List<BlobDto>> ListAsync()
        {
            // Get a reference to a container named in appsettings.json
            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);

            // Create a new list object for 
            List<BlobDto> files = new List<BlobDto>();

            await foreach (BlobItem file in container.GetBlobsAsync())
            {
                // Add each file retrieved from the storage container to the files list by creating a BlobDto object
                string uri = container.Uri.ToString();
                var name = file.Name;
                var fullUri = $"{uri}/{name}";

                files.Add(new BlobDto
                {
                    Uri = fullUri,
                    Name = name,
                    ContentType = file.Properties.ContentType
                });
            }

            // Return all files to the requesting method
            return files;
        }

        public async Task<BlobResponseDto> UploadAsync(IFormFile blob )
        {

            string _storageConnectionString = _appSettings.BlobConnectionString;
            string _storageContainerName = _appSettings.BlobContainerName;
            // Create new upload response object that we can return to the requesting method
            BlobResponseDto response = new();



            // Get a reference to a container named in appsettings.json and then create it
            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);

            // Create the container if it does not exist
            await container.CreateIfNotExistsAsync();

            try
            {
                // Get a reference to the blob just uploaded from the API in a container from configuration settings
                BlobClient client = container.GetBlobClient(blob.FileName);

                // Open a stream for the file we want to upload
                await using (Stream? data = blob.OpenReadStream())
                {
                    // Upload the file async
                    await client.UploadAsync(data);
                }

                // Everything is OK and file got uploaded
                response.Status = $"File {blob.FileName} Uploaded Successfully";
                response.Error = false;
                response.Blob.Uri = client.Uri.AbsoluteUri;
                response.Blob.Name = client.Name;

            }
            // If the file already exists, we catch the exception and do not upload it
            catch (RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                _logger.LogError($"File with name {blob.FileName} already exists in container. Set another name to store the file in the container: '{_storageContainerName}.'");
                response.Status = $"File with name {blob.FileName} already exists. Please use another name to store your file.";
                response.Error = true;
                return response;
            }
            // If we get an unexpected error, we catch it here and return the error message
            catch (RequestFailedException ex)
            {
                // Log error to console and create a new response we can return to the requesting method
                _logger.LogError($"Unhandled Exception. ID: {ex.StackTrace} - Message: {ex.Message}");
                response.Status = $"Unexpected error: {ex.StackTrace}. Check log with StackTrace ID.";
                response.Error = true;
                return response;
            }

            // Return the BlobUploadResponse object
            return response;
        }

        //public async Task<BlobResponseDto> Uploadedvideos(VedioUpload lessonViewDTO, TrailVideoUpload trailVideoUploadDTO, int lessonSetupID, string button, IFormFile blob)
        //{
        //    string _storageConnectionString = _appSettings.BlobConnectionString;
        //    string _storageContainerName = "Uploads" + "/" + lessonViewDTO.LanguageId + "/" + lessonSetupID;
        //    // Create new upload response object that we can return to the requesting method
        //    BlobResponseDto response = new();



        //    // Get a reference to a container named in appsettings.json and then create it
        //    BlobContainerClient container = new BlobContainerClient(_storageConnectionString, _storageContainerName);

        //    // Create the container if it does not exist
        //    await container.CreateIfNotExistsAsync();

        //    try
        //    {
        //        // Get a reference to the blob just uploaded from the API in a container from configuration settings
        //        BlobClient client = container.GetBlobClient(blob.FileName);

        //        // Open a stream for the file we want to upload
        //        await using (Stream? data = blob.OpenReadStream())
        //        {
        //            // Upload the file async
        //            await client.UploadAsync(data);
        //        }

        //        // Everything is OK and file got uploaded
        //        response.Status = $"File {blob.FileName} Uploaded Successfully";
        //        response.Error = false;
        //        response.Blob.Uri = client.Uri.AbsoluteUri;
        //        response.Blob.Name = client.Name;

        //    }
        //    // If the file already exists, we catch the exception and do not upload it
        //    catch (RequestFailedException ex)
        //        when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
        //    {
        //        _logger.LogError($"File with name {blob.FileName} already exists in container. Set another name to store the file in the container: '{_storageContainerName}.'");
        //        response.Status = $"File with name {blob.FileName} already exists. Please use another name to store your file.";
        //        response.Error = true;
        //        return response;
        //    }
        //    // If we get an unexpected error, we catch it here and return the error message
        //    catch (RequestFailedException ex)
        //    {
        //        // Log error to console and create a new response we can return to the requesting method
        //        _logger.LogError($"Unhandled Exception. ID: {ex.StackTrace} - Message: {ex.Message}");
        //        response.Status = $"Unexpected error: {ex.StackTrace}. Check log with StackTrace ID.";
        //        response.Error = true;
        //        return response;
        //    }

        //    // Return the BlobUploadResponse object
        //    return response;
        //}
       

        public BlobResponseDto DeleteFile(TrailVideoUpload trailVideoUploadDTO, string fileName, int LessonSetupID)
        {
            string blobstorageconnection = _appSettings.BlobConnectionString;
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(blobstorageconnection);
            string PathSaved = "videos-upload-" + trailVideoUploadDTO.LanguageId;
          
            PathSaved = PathSaved.ToLower();
            CloudBlobClient cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            string strContainerName = PathSaved;
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(strContainerName);
            string FullFileName = LessonSetupID + "-" + fileName;
            var blobs = cloudBlobContainer.GetBlobReference(FullFileName);
            blobs.DeleteIfExistsAsync();
          //  blobs.DeleteAsync();
            //var check = "";
            // return check;
            //return response;
            return null;
        }
        public    BlobResponseDto Uploadedvideosforazure(VedioUpload lessonViewDTO, TrailVideoUpload trailVideoUploadDTO, MobileVideoUpload PostMobileLessonDetailDTO, string button, int lessonSetupID, IFormFile blob,string functionName)
        {
            string _storageConnectionString = _appSettings.BlobConnectionString;
            string azure_path = _appSettings.Azure_Path;

            if (trailVideoUploadDTO.trailvideodata!=null)
            {
                blob = trailVideoUploadDTO.trailvideodata;
            }

            // Create new upload response object that we can return to the requesting method
            BlobResponseDto response = new();

             string PathSaved = "videos-upload-" +lessonViewDTO.LanguageId ;
           //string PathSaved = "videos-upload-" + lessonViewDTO.LanguageId + "-" + lessonSetupID;
            PathSaved = PathSaved.ToLower();

            if (functionName == "Edit")
            {
                // right code for delete here 
                // await  DeleteFile(lessonViewDTO,"");
              var delete =  DeleteFile(trailVideoUploadDTO, blob.FileName, lessonSetupID);
            }

            //CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(filename);

            // Get a reference to a container named in appsettings.json and then create it
            BlobContainerClient container = new BlobContainerClient(_storageConnectionString, PathSaved);
     

            // Create the container if it does not exist
            container.CreateIfNotExistsAsync(PublicAccessType.Blob);

            try
            {
                //for full video
                if (lessonViewDTO.videodata!=null)
                {
                    var fullpath = Uploadvideooncontainer(lessonViewDTO, lessonViewDTO.videodata, container, lessonSetupID);
                    response.fullpath = fullpath;
                }
                // var fullpath=   Uploadvideooncontainer(lessonViewDTO, lessonViewDTO.videodata, container, lessonSetupID);               
                //for trial video
                if (trailVideoUploadDTO.trailvideodata!=null)
                {


                    var trialpath = Uploadvideooncontainer(lessonViewDTO, trailVideoUploadDTO.trailvideodata, container, lessonSetupID);
                    response.trialpath = trialpath;
                }

                if (PostMobileLessonDetailDTO.mobilevideodata != null)
                {


                    var mobilepath = Uploadvideooncontainer(lessonViewDTO, PostMobileLessonDetailDTO.mobilevideodata, container, lessonSetupID);
                    response.mobilepath = mobilepath;
                }

                var lessonsetupidint = Convert.ToString(lessonSetupID);
                // new function
                //string path = UploadvideooncontainerNew(lessonViewDTO, trailVideoUploadDTO.trailvideodata, container, lessonsetupidint);

                // Everything is OK and file got uploaded
                response.Status = $"File {blob.FileName} Uploaded Successfully";
                response.Error = false;

               // response.fullpath = fullpath;
                //response.trialpath = trialpath;
                // response.Blob.Uri = client.Uri.AbsoluteUri;
                //   response.Blob.Name = client.Name;

            }
            // If the file already exists, we catch the exception and do not upload it
            catch (RequestFailedException ex)
                when (ex.ErrorCode == BlobErrorCode.BlobAlreadyExists)
            {
                _logger.LogError($"File with name {blob.FileName} already exists in container. Set another name to store the file in the container: '{_storageContainerName}.'");
                response.Status = $"File with name {blob.FileName} already exists. Please use another name to store your file.";
                response.Error = true;
                return response;
            }
            // If we get an unexpected error, we catch it here and return the error message
            catch (RequestFailedException ex)
            {
                // Log error to console and create a new response we can return to the requesting method
                _logger.LogError($"Unhandled Exception. ID: {ex.StackTrace} - Message: {ex.Message}");
                response.Status = $"Unexpected error: {ex.StackTrace}. Check log with StackTrace ID.";
                response.Error = true;
                return response;
            }

            // Return the BlobUploadResponse object
            return response;
        }

        private static string Uploadvideooncontainer(VedioUpload lessonViewDTO, IFormFile blob, BlobContainerClient container, int LessonSetupID)
        {
            //for uploading final video
            // Get a reference to the blob just uploaded from the API in a container from configuration settings
            string FullFileName = LessonSetupID + "-"+blob.FileName;
            BlobClient client = container.GetBlobClient(FullFileName);
            // BlobClient client = container.GetBlobReference(blob);

            // Open a stream for the file we want to upload


            using (Stream? data = blob.OpenReadStream())
            {
                // Upload the file async
                client.Upload(data);
            }

            return client.Uri.LocalPath;
        }

        private  string UploadvideooncontainerNew(VedioUpload lessonViewDTO, IFormFile blob, BlobContainerClient container, string lessonSetupID)
        {
            //string UploadedPath = "";

            string filename = null;
            string uploadDir = "";
            string targetfoldername = "";
            if (lessonViewDTO.videodata != null)
            {
                // targetfoldername= "Uploads" + "/" + DateTime.Now.ToString("ddmmyyyy");
                targetfoldername = "Uploads" + "/" + lessonViewDTO.LanguageId + "/" + lessonSetupID;
                uploadDir = _appSettings.Azure_Path + targetfoldername;

                // BlobContainerClient CreateNewContainer = new BlobContainerClient(_storageConnectionString, uploadDir);

                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }
                string fullName = "Full";
                string strFinalFileName = Path.GetFileName(lessonViewDTO.videodata.FileName);

                // string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Videos"+DateTime.Now.ToString());
                // filename = lessonViewDTO.LanguageId + "_" + Guid.NewGuid().ToString() + "_" + strFinalFileName;
                filename = lessonViewDTO.LanguageId + "_" + lessonSetupID + "_" + fullName + "_" + strFinalFileName;
                string filepath = Path.Combine(uploadDir, filename);
                using (var fileStream = new FileStream(filepath, FileMode.Create))
                {
                    lessonViewDTO.videodata.CopyTo(fileStream);
                }
            }
            return targetfoldername + "/" + filename;
            ////for uploading final video
            // Get a reference to the blob just uploaded from the API in a container from configuration settings
            BlobClient client = container.GetBlobClient(blob.FileName);
            // BlobClient client = container.GetBlobReference(blob);

            // Open a stream for the file we want to upload


            using (Stream? data = blob.OpenReadStream())
            {
                // Upload the file async
                client.Upload(data);
            }
            // return UploadedPath;
           // return null;
        }

        Task<BlobResponseDto> IAzureStorage.Uploadedvideos(VedioUpload lessonViewDTO, TrailVideoUpload trailVideoUploadDTO, string button, int lessonSetupID, IFormFile blob)
        {
            throw new NotImplementedException();
        }
    }
}
