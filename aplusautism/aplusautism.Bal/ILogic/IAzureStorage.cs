using aplusautism.Bal.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace aplusautism.Bal.ILogic
{
    public interface IAzureStorage
    {
        /// <summary>
        /// This method uploads a file submitted with the request
        /// </summary>
        /// <param name="file">File for upload</param>
        /// <returns>Blob with status</returns>
        /// 

          Task<BlobResponseDto> Uploadedvideos(VedioUpload lessonViewDTO, TrailVideoUpload trailVideoUploadDTO, string button, int lessonSetupID, IFormFile blob);
        Task<BlobResponseDto> UploadAsync(IFormFile file);

        /// <summary>
        /// This method downloads a file with the specified filename
        /// </summary>
        /// <param name="blobFilename">Filename</param>
        /// <returns>Blob</returns>
        Task<BlobDto> DownloadAsync(string blobFilename);

        /// <summary>
        /// This method deleted a file with the specified filename
        /// </summary>
        /// <param name="blobFilename">Filename</param>
        /// <returns>Blob with status</returns>
        Task<BlobResponseDto> DeleteAsync(string blobFilename);

        /// <summary>
        /// This method returns a list of all files located in the container
        /// </summary>
        /// <returns>Blobs in a list</returns>
        Task<List<BlobDto>> ListAsync();

        BlobResponseDto Uploadedvideosforazure(VedioUpload lessonViewDTO, TrailVideoUpload trailVideoUploadDTO, MobileVideoUpload PostMobileLessonDetailDTO, string button, int lessonSetupID, IFormFile blob,string functionName);

        //BlobResponseDto DeleteFile(VedioUpload lessonViewDTO, string fileName);
    }
}
