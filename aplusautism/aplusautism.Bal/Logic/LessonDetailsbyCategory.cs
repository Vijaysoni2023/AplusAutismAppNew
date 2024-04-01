using aplusautism.Bal.DTO;
using aplusautism.Bal.ILogic;
using aplusautism.Common.Enums;
using aplusautism.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Bal.Logic
{
    public class LessonDetailsbyCategory : IlessondetailsBycategory
    {

        public readonly IRepository<LessonDetailsbycategoryDTO> _LessonDetailsbycategoryDTO;

        public readonly IRepository<PaymentsDTO> _PaymentsDTO;  

        public LessonDetailsbyCategory(IRepository<LessonDetailsbycategoryDTO> LessonDetailsbycategoryDTO, IRepository<PaymentsDTO> paymentsDTO)
        {
            _LessonDetailsbycategoryDTO = LessonDetailsbycategoryDTO;
            _PaymentsDTO = paymentsDTO;
        }
        public List<LessonDetailsbycategoryDTO> GetlessonBycategory(string opcode , string UserLanguageId, string LessonName,string userId, string userMainId,int UserStatus)
        {
            List<LessonDetailsbycategoryDTO> Listdata = new List<LessonDetailsbycategoryDTO>();
            try
            {
                string procName = SPROC_Names.sp_getLesson_forclientbycategory.ToString();
                var ParamsArray = new SqlParameter[6];
                ParamsArray[0] = new SqlParameter() { ParameterName = "@LessonId", Value = UserLanguageId, DbType = System.Data.DbType.String };
                ParamsArray[1] = new SqlParameter() { ParameterName = "@Lessoncategoryid", Value = LessonName, DbType = System.Data.DbType.Int32 };
                ParamsArray[2] = new SqlParameter() { ParameterName = "@opcode", Value = opcode, DbType = System.Data.DbType.String };
                ParamsArray[3] = new SqlParameter() { ParameterName = "@UserId", Value = userId, DbType = System.Data.DbType.String };
                ParamsArray[4] = new SqlParameter() { ParameterName = "@UserMainID", Value = userMainId, DbType = System.Data.DbType.String };
                ParamsArray[5] = new SqlParameter() { ParameterName = "@UserStatusValue", Value = UserStatus, DbType = System.Data.DbType.Int32 };
                
                var resultData = _LessonDetailsbycategoryDTO.ExecuteWithJsonResult(procName, "LessonDetailsBycategory", ParamsArray);

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

        public List<PaymentsDTO> GetUserPaymentStatus(string opcode, string userId, string userMainId)
        {
            List<PaymentsDTO> Listdata = new List<PaymentsDTO>();
            try
            {
                string procName = SPROC_Names.USP_GetUserPaymentStatus.ToString();
                var ParamsArray = new SqlParameter[3];
                ParamsArray[0] = new SqlParameter() { ParameterName = "@opcode", Value = opcode, DbType = System.Data.DbType.String };
                ParamsArray[1] = new SqlParameter() { ParameterName = "@userId", Value = userId, DbType = System.Data.DbType.DateTime };
                ParamsArray[2] = new SqlParameter() { ParameterName = "@userMainId", Value = userMainId, DbType = System.Data.DbType.DateTime };
                var resultData = _PaymentsDTO.ExecuteWithJsonResult(procName, "PaymentsDTO", ParamsArray);

                if (resultData != null)
                {
                    return resultData != null ? resultData : resultData.ToList();
                }
                else
                {
                    return Listdata;
                }
            }
            catch (ApplicationException)
            {
                throw;
            }
        }

    }
}
