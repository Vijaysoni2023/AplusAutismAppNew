using aplusautism.Bal.DTO;
using aplusautism.Data.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Bal.ILogic
{
    public interface IGlobalCodeCategorylogic
    {


        List<Countries> GetCountries();
        List<States> Getstate(int countryid);

        List<Cities> GetcityList(int countryid,int stateid);

        List<GlobalCodeCategory> GetAll();
        List<LessonSetup> GetLessonSetupDetail();
        string UpdateLessonDetail(VedioUpload postLessonDetailDTO, TrailVideoUpload trailVideoUploadDTO, MobileVideoUpload PostMobileLessonDetailDTO, string status);
        List<LessonSetup> GetLessonEditDetail(string pGlobalCodeCategoryValue, int pOpCode, string pOpParams, int pLoggedInUser, string pLNCode);
        List<LessonSetup> GetSearchLetter(string pGlobalCodeCategoryValue, string pOpCode, string pOpParams, int pLoggedInUser, string pLNCode);
        List<GlobalCodes> GetGlobalCodesByGlobalCodeCategoryValue(string pGlobalCodeCategoryValue, string pOpCode, string pOpParams, string pLoggedInUser, string pLNCode);
        string InsertLesson(VedioUpload lessonViewDTO, TrailVideoUpload trailVideoUploadDTO, MobileVideoUpload PostMobileLessonDetailDTO, string status);
        string RemoveLesson(string pGlobalCodeCategoryValue, string pOpCode, string pOpParams, int pLoggedInUser, string pLNCode);

        // function for getting client Status
        List<GlobalCodes> GetClientStatus();

        List<GlobalCodesDTO> GetClientStatusList(string opcode);

    }
}
