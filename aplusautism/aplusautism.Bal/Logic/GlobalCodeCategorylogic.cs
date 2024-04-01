using aplusautism.Bal.DTO;
using aplusautism.Bal.ILogic;
using aplusautism.Common;
using aplusautism.Common.Enums;
using aplusautism.Data.Models;
using aplusautism.Repository.Repository;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Hosting;
using aplusautism.Data;

namespace aplusautism.Bal.Logic
{
    public class GlobalCodeCategorylogic : IGlobalCodeCategorylogic
    {
        public readonly IRepository<GlobalCodesDTO> _GlobalCodesDTO;
        private readonly IRepository<GlobalCodeCategory> _globalCodeCategory;
        private readonly IRepository<LessonSetup> _lessonsetup;
        private readonly IRepository<GlobalCodes> _globalcodes;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AplusautismDbContext _dbcontext;
        private readonly IAzureStorage _storage;
        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="globalCategory"></param>
        public GlobalCodeCategorylogic(IRepository<GlobalCodeCategory> globalCategory, IAzureStorage storage,
            IRepository<LessonSetup> lessonsetup, IRepository<GlobalCodes> globalcodes, IWebHostEnvironment webHostEnvironment, AplusautismDbContext dbcontext, IRepository<GlobalCodesDTO> globalCodesDTO)
        {
            _storage = storage;
            _globalCodeCategory = globalCategory;
            _lessonsetup = lessonsetup;
            _globalcodes = globalcodes;
            _webHostEnvironment = webHostEnvironment;
            _dbcontext = dbcontext;
            _GlobalCodesDTO = globalCodesDTO;
        }
        /// <summary>
        /// Get All global Code Categories
        /// </summary>
        /// <returns></returns>
        public List<GlobalCodeCategory> GetAll()
        {
            //var gccList =  _globalCodeCategory.GetAll().ToList();
            //return gccList;
            try
            {
                string procName = SPROC_Names.sp_getGlobalCode.ToString();
                var ParamsArray = new SqlParameter[1];
                ParamsArray[0] = new SqlParameter() { ParameterName = "@GlobalId", Value = "", DbType = System.Data.DbType.String };
                var resultData = _globalCodeCategory.ExecuteWithJsonResult(procName, "GlobalCodeCategory", ParamsArray);

                return resultData.ToList();
            }
            catch (ApplicationException ex)
            {
                throw;
            }
        }
        public List<LessonSetup> GetLessonSetupDetail()
        {
            //var gccList =  _globalCodeCategory.GetAll().ToList();
            //return gccList;
            List<LessonSetup> resultlist = new List<LessonSetup>();
            try
            {
                string procName = SPROC_Names.sp_getLesson.ToString();
                var ParamsArray = new SqlParameter[1];
                ParamsArray[0] = new SqlParameter() { ParameterName = "@LessonId", Value = "", DbType = System.Data.DbType.String };
                var resultData = _lessonsetup.ExecuteWithJsonResult(procName, "LessonSetup", ParamsArray);

                if (resultData != null)
                {
                    return resultData.ToList();
                }
                return resultlist;
            }
            catch (ApplicationException ex)
            {
                throw;
            }
        }
        public List<GlobalCodes> GetGlobalCodesByGlobalCodeCategoryValue(string pGlobalCodeCategoryValue, string pOpCode, string pOpParams, string pLoggedInUser, string pLNCode)
        {
            try
            {
                string procName = SPROC_Names.USP_GetGlobalDataByCategoryIds.ToString();
                var ParamsArray = new SqlParameter[5];
                ParamsArray[0] = new SqlParameter() { ParameterName = "@pGlobalCodeCategoryValue", Value = pGlobalCodeCategoryValue, DbType = System.Data.DbType.String };
                ParamsArray[1] = new SqlParameter() { ParameterName = "@pOpCode", Value = pOpCode, DbType = System.Data.DbType.String };
                ParamsArray[2] = new SqlParameter() { ParameterName = "@pOpParams", Value = pOpParams, DbType = System.Data.DbType.String };
                ParamsArray[3] = new SqlParameter() { ParameterName = "@pLoggedInUser", Value = pLoggedInUser, DbType = System.Data.DbType.String };
                ParamsArray[4] = new SqlParameter() { ParameterName = "@pLNCode", Value = pLNCode, DbType = System.Data.DbType.String };
                var resultData = _globalcodes.ExecuteWithJsonResult(procName, "GlobalCodes", ParamsArray);

                return resultData;
            }
            catch (ApplicationException ex)
            {
                throw;
            }
        }
        public   string InsertLesson(VedioUpload lessonViewDTO, TrailVideoUpload trailVideoUploadDTO, MobileVideoUpload PostMobileLessonDetailDTO, string status)
        {
          
            try
            {
                //var VideoStrpath = Uploadfile(lessonViewDTO);

                //var VideoTrailStrpath = UploadTrailfile(trailVideoUploadDTO);

                var VideoStrpath = "";
                var VideoTrailStrpath = "";
                var fullpath = "";
                var trialpath = "";
                var trialvideopath = "";
                var mobilepath = "";
                LessonSetup lesson = new LessonSetup
                {

                    CreatedBy = 1,
                    IsDeleted = false,
                    LessonStatus = status,
                    CreatedDate = CommonFunction.CurrentDate,
                    LessonDescription = lessonViewDTO.Description,
                    LessonTitle = lessonViewDTO.Manage,
                    LessonLanguage = lessonViewDTO.LanguageId,
                    LessonCategoryId = lessonViewDTO.LessonCategoryId,
                    LessonVideoPath = fullpath,
                    LessonTrailVideoPath = mobilepath,
                    MobileVideoPath= trialpath,
                    SortOrder = lessonViewDTO.SortOrder




                };
                _lessonsetup.Insert(lesson);


                // Uploaloading vedio and Update Paths in Table
                var lessonSetupID = lesson.LessonSetupID;
                //for uploading in azure blob container
                //  BlobResponseDto? response =  _storage.UploadAsync(img_Upload);
                var lessonsetupidint = Convert.ToInt32(lessonSetupID);
                string Update = "Add";
                var responsedata= CallingAzureblobStorageAsync(lessonViewDTO, trailVideoUploadDTO, PostMobileLessonDetailDTO, lessonsetupidint, Update);//hold for upload

              // VideoStrpath = Uploadfile(lessonViewDTO, Convert.ToInt32(lessonSetupID));
              //  VideoTrailStrpath = UploadTrailfile(trailVideoUploadDTO, Convert.ToInt32(lessonSetupID));

                var editeddata = _dbcontext.LessonSetup.Where(i => i.LessonSetupID == lessonSetupID).FirstOrDefault();
                //editeddata.LessonVideoPath = VideoStrpath;
                //editeddata.LessonTrailVideoPath = VideoTrailStrpath;
                //if (lessonViewDTO.Manage=="Zoo Animals")
                //{
                //    editeddata.LessonTrailVideoPath = "/videos-upload-trialvideo/Trialvideo-zoo.mp4";

                //}
                //else
                //        {
                //    editeddata.LessonTrailVideoPath = "/videos-upload-trialvideo/Final Trial vedio.mp4";
                //}
                editeddata.LessonVideoPath =  responsedata.fullpath;
                editeddata.LessonTrailVideoPath = responsedata.mobilepath;
                editeddata.MobileVideoPath =  responsedata.trialpath;
                _dbcontext.SaveChanges();


                return "Success";
            }
            catch (ApplicationException ex)
            {
                return "Fail";
            }
        }

        private BlobResponseDto CallingAzureblobStorageAsync(VedioUpload lessonViewDTO, TrailVideoUpload trailVideoUploadDTO, MobileVideoUpload PostMobileLessonDetailDTO, int lessonsetupidint, string functionName)
        {
            BlobResponseDto? responsed = _storage.Uploadedvideosforazure(lessonViewDTO, trailVideoUploadDTO, PostMobileLessonDetailDTO, " ", lessonsetupidint, trailVideoUploadDTO.videodata , functionName);
            return responsed;
        }


        private string Uploadfile(VedioUpload lessonViewDTO, int  lessonSetupID)
        {
            string filename = null;
            string uploadDir = "";
            string targetfoldername = "";
            if (lessonViewDTO.videodata != null)
            {
                // targetfoldername= "Uploads" + "/" + DateTime.Now.ToString("ddmmyyyy");
                targetfoldername = "Uploads" + "/" + lessonViewDTO.LanguageId + "/" + lessonSetupID;
                uploadDir = _webHostEnvironment.WebRootPath + "/" + targetfoldername;
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
            return targetfoldername + "/"+ filename;
        }


        private string UploadTrailfile(TrailVideoUpload lessonViewDTO, int lessonSetupID)
        {
            string filename = null;
            string uploadDir = "";
            string targetfoldername = "";
            if (lessonViewDTO.trailvideodata != null)
            {
                // targetfoldername = "Uploads" + "/" + DateTime.Now.ToString("ddmmyyyy");
                targetfoldername = "Uploads" + "/" + lessonViewDTO.LanguageId + "/" + lessonSetupID;
                uploadDir = _webHostEnvironment.WebRootPath + "/" + targetfoldername;
                if (!Directory.Exists(uploadDir))
                {
                    Directory.CreateDirectory(uploadDir);
                }

                string strFinalFileName = Path.GetFileName(lessonViewDTO.trailvideodata.FileName);
                string fullName = "Trail";
                // string uploadDir = Path.Combine(_webHostEnvironment.WebRootPath, "Videos"+DateTime.Now.ToString());
                //  filename = lessonViewDTO.LanguageId + "_" + Guid.NewGuid().ToString() + "_" + strFinalFileName;
                filename = lessonViewDTO.LanguageId + "_" + lessonSetupID + "_" + fullName + "_" + strFinalFileName;
                string filepath = Path.Combine(uploadDir, filename);
                using (var fileStream = new FileStream(filepath, FileMode.Create))
                {
                    lessonViewDTO.trailvideodata.CopyTo(fileStream);
                }
            }
            return targetfoldername + "/" + filename;
        }
        public string RemoveLesson(string pGlobalCodeCategoryValue, string pOpCode, string pOpParams, int pLoggedInUser, string pLNCode)
        {
            try
            {
                string procName = SPROC_Names.USP_GetGlobalDataByCategoryIds.ToString();
                var ParamsArray = new SqlParameter[5];
                ParamsArray[0] = new SqlParameter() { ParameterName = "@pGlobalCodeCategoryValue", Value = pGlobalCodeCategoryValue, DbType = System.Data.DbType.String };
                ParamsArray[1] = new SqlParameter() { ParameterName = "@pOpCode", Value = pOpCode, DbType = System.Data.DbType.String };
                ParamsArray[2] = new SqlParameter() { ParameterName = "@pOpParams", Value = pOpParams, DbType = System.Data.DbType.String };
                ParamsArray[3] = new SqlParameter() { ParameterName = "@pLoggedInUser", Value = pLoggedInUser, DbType = System.Data.DbType.String };
                ParamsArray[4] = new SqlParameter() { ParameterName = "@pLNCode", Value = pLNCode, DbType = System.Data.DbType.String };
                var resultData = _lessonsetup.ExecuteWithJsonResult(procName, "AB_User", ParamsArray);
                var isdeleted = resultData.FirstOrDefault().IsDeleted;
                if (isdeleted == true)
                {
                    return "Success";
                }
                return "Faild";
            }
            catch (ApplicationException ex)
            {
                throw;
            }
        }
        public List<LessonSetup> GetSearchLetter(string pGlobalCodeCategoryValue, string pOpCode, string pOpParams, int pLoggedInUser, string pLNCode)
        {
            try
            {
                string procName = SPROC_Names.USP_GetGlobalDataByCategoryIds.ToString();
                var ParamsArray = new SqlParameter[5];
                ParamsArray[0] = new SqlParameter() { ParameterName = "@pGlobalCodeCategoryValue", Value = pGlobalCodeCategoryValue, DbType = System.Data.DbType.String };
                ParamsArray[1] = new SqlParameter() { ParameterName = "@pOpCode", Value = pOpCode, DbType = System.Data.DbType.String };
                ParamsArray[2] = new SqlParameter() { ParameterName = "@pOpParams", Value = pOpParams, DbType = System.Data.DbType.String };
                ParamsArray[3] = new SqlParameter() { ParameterName = "@pLoggedInUser", Value = pLoggedInUser, DbType = System.Data.DbType.String };
                ParamsArray[4] = new SqlParameter() { ParameterName = "@pLNCode", Value = pLNCode, DbType = System.Data.DbType.String };
                var resultData = _lessonsetup.ExecuteWithJsonResult(procName, "LessonSetup", ParamsArray);

                return resultData;
            }
            catch (ApplicationException ex)
            {
                throw;
            }
        }
        public List<LessonSetup> GetLessonEditDetail(string pGlobalCodeCategoryValue, int pOpCode, string pOpParams, int pLoggedInUser, string pLNCode)
        {
            try
            {
                string procName = SPROC_Names.USP_GetGlobalDataByCategoryIds.ToString();
                var ParamsArray = new SqlParameter[5];
                ParamsArray[0] = new SqlParameter() { ParameterName = "@pGlobalCodeCategoryValue", Value = pGlobalCodeCategoryValue, DbType = System.Data.DbType.String };
                ParamsArray[1] = new SqlParameter() { ParameterName = "@pOpCode", Value = pOpCode, DbType = System.Data.DbType.String };
                ParamsArray[2] = new SqlParameter() { ParameterName = "@pOpParams", Value = pOpParams, DbType = System.Data.DbType.String };
                ParamsArray[3] = new SqlParameter() { ParameterName = "@pLoggedInUser", Value = pLoggedInUser, DbType = System.Data.DbType.String };
                ParamsArray[4] = new SqlParameter() { ParameterName = "@pLNCode", Value = pLNCode, DbType = System.Data.DbType.String };
                var resultData = _lessonsetup.ExecuteWithJsonResult(procName, "LessonSetup", ParamsArray);

                return resultData;
            }
            catch (ApplicationException ex)
            {
                throw;
            }
        }
        public string UpdateLessonDetail(VedioUpload postLessonDetailDTO, TrailVideoUpload trailVideoUploadDTO, MobileVideoUpload PostMobileLessonDetailDTO, string status)
        {

            try
            {
                int lessonsetupidint = Convert.ToInt32(postLessonDetailDTO.LessonSetupId);


                string Update = "Edit";
                if (postLessonDetailDTO.videodata != null || trailVideoUploadDTO.trailvideodata != null)
                {
                    // var responsedata = CallingAzureblobStorageAsync(postLessonDetailDTO, trailVideoUploadDTO, lessonsetupidint, Update);//hold for upload



                    var responsedata = CallingAzureblobStorageAsync(postLessonDetailDTO, trailVideoUploadDTO, PostMobileLessonDetailDTO, lessonsetupidint, Update);//hold for upload
                    var existingediteddata = _dbcontext.LessonSetup.Where(i => i.LessonSetupID == postLessonDetailDTO.LessonSetupId).FirstOrDefault();
                    // var VideoStrpath = Uploadfile(postLessonDetailDTO,Convert.ToInt32(postLessonDetailDTO.LessonSetupId));
                    //  var VideoTrailStrpath = UploadTrailfile(trailVideoUploadDTO, Convert.ToInt32(postLessonDetailDTO.LessonSetupId));
                   // var VideoStrpath = existingediteddata.LessonVideoPath;
                   // var VideoTrailStrpath = existingediteddata.MobileVideoPath;
                    var VideoStrpath = responsedata.fullpath;
                    var VideoTrailStrpath =responsedata.trialpath;


                    //if (!string.IsNullOrEmpty(VideoStrpath.ToString()) && VideoStrpath.ToString() != null &&
                    //    !string.IsNullOrEmpty(VideoTrailStrpath.ToString()) && VideoTrailStrpath.ToString() != null)
                    if(VideoStrpath != null && VideoTrailStrpath!=null)
                    {
                        var editeddata = _dbcontext.LessonSetup.Where(i => i.LessonSetupID == postLessonDetailDTO.LessonSetupId).FirstOrDefault();
                        editeddata.LessonTitle = postLessonDetailDTO.Manage;
                        editeddata.LessonCategoryId = postLessonDetailDTO.LessonCategoryId;
                        editeddata.LessonDescription = postLessonDetailDTO.Description;
                        // editeddata.LessonVideoPath = VideoStrpath;
                        //editeddata.LessonTrailVideoPath = VideoTrailStrpath;
                        editeddata.LessonVideoPath = responsedata.fullpath;
                        editeddata.MobileVideoPath = responsedata.trialpath;
                        editeddata.ModifiedDate = DateTime.Now;
                        editeddata.SortOrder = postLessonDetailDTO.SortOrder;
                    }
                    //else if ((!string.IsNullOrEmpty(VideoStrpath.ToString()) && VideoStrpath.ToString() != null) ||
                    //   (string.IsNullOrEmpty(VideoTrailStrpath.ToString()) && VideoTrailStrpath.ToString() != null))
                    else if(VideoTrailStrpath == null && VideoStrpath != null)
                    {

                        var editeddata = _dbcontext.LessonSetup.Where(i => i.LessonSetupID == postLessonDetailDTO.LessonSetupId).FirstOrDefault();

                        editeddata.LessonTitle = postLessonDetailDTO.Manage;
                        editeddata.LessonCategoryId = postLessonDetailDTO.LessonCategoryId;
                        editeddata.LessonDescription = postLessonDetailDTO.Description;
                        // editeddata.LessonVideoPath = VideoStrpath;
                        editeddata.LessonVideoPath = responsedata.fullpath;
                        editeddata.ModifiedDate = DateTime.Now;
                        editeddata.SortOrder = postLessonDetailDTO.SortOrder;
                    }
                    //else if ((string.IsNullOrEmpty(VideoStrpath.ToString()) && VideoStrpath.ToString() != null) ||
                    //    (!string.IsNullOrEmpty(VideoTrailStrpath.ToString()) && VideoTrailStrpath.ToString() != null))
                    else if(VideoTrailStrpath != null && VideoStrpath == null)
                    {
                        var editeddata = _dbcontext.LessonSetup.Where(i => i.LessonSetupID == postLessonDetailDTO.LessonSetupId).FirstOrDefault();

                        editeddata.LessonTitle = postLessonDetailDTO.Manage;
                        editeddata.LessonCategoryId = postLessonDetailDTO.LessonCategoryId;
                        editeddata.LessonDescription = postLessonDetailDTO.Description;
                        editeddata.SortOrder = postLessonDetailDTO.SortOrder;
                        // editeddata.LessonTrailVideoPath = VideoTrailStrpath;
                        editeddata.MobileVideoPath = responsedata.trialpath;

                        editeddata.ModifiedDate = DateTime.Now;
                    }
                }

                else
                {
                    var editeddata = _dbcontext.LessonSetup.Where(i => i.LessonSetupID == postLessonDetailDTO.LessonSetupId).FirstOrDefault();

                    editeddata.LessonTitle = postLessonDetailDTO.Manage;
                    editeddata.LessonCategoryId = postLessonDetailDTO.LessonCategoryId;
                    editeddata.LessonDescription = postLessonDetailDTO.Description;
                    editeddata.SortOrder = postLessonDetailDTO.SortOrder;
                    editeddata.ModifiedDate = DateTime.Now;
                    editeddata.LessonStatus = trailVideoUploadDTO.status;
                }
                _dbcontext.SaveChanges();
                return "Updated";


            }
            catch (ApplicationException ex)
            {
                return "Faild";
            }
        }

        public List<Countries> GetCountries()
        {
            
            return _dbcontext.Countries.OrderBy(x => x.Name).ToList(); //_dbcontext.Countries.ToList();
        }
        // Get list of status of clients
        public List<GlobalCodes> GetClientStatus()
        {
            return _dbcontext.globalCodes.Where(i => i.GlobalCodeCategoryValue == "10").ToList();
        }

        public List<GlobalCodesDTO> GetClientStatusList(string opcode)
        {
            List<GlobalCodesDTO> Listdata = new List<GlobalCodesDTO>();
            try
            {
                string procName = SPROC_Names.USP_GetClientStatusList.ToString();
                var ParamsArray = new SqlParameter[1];
                ParamsArray[0] = new SqlParameter() { ParameterName = "@opcode", Value = opcode, DbType = System.Data.DbType.String };
                var resultData = _GlobalCodesDTO.ExecuteWithJsonResult(procName, "GlobalCodesDTO", ParamsArray);

                if (resultData != null)
                {
                    return resultData != null ? resultData : resultData.ToList();
                }
                else
                {
                    return Listdata;
                }
            }
            catch (ApplicationException ex)
            {
                throw;
            }
        }

        public List<States> Getstate(int countryid)
        {
            return _dbcontext.States.Where(i => i.CountryId == countryid).ToList();
        }

        public List<Cities> GetcityList(int countryid, int stateid)
        {
            return _dbcontext.Cities.Where(i => i.StateId == stateid).ToList();
        }
    }
}
