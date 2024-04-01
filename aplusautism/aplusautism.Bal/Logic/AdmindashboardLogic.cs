using aplusautism.Bal.DTO;
using aplusautism.Bal.ILogic;
using aplusautism.Common.Enums;
using aplusautism.Data;
using aplusautism.Data.Models;
using aplusautism.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Bal.Logic
{
    public class AdmindashboardLogic : Iadmindashboard
    {

        public AplusautismDbContext _dbcontext;

        private readonly IRepository<ClientActiveCountsDTO> _ClientActiveCountsDTO;
        public AdmindashboardLogic(AplusautismDbContext dbcontext, IRepository<ClientActiveCountsDTO> ClientActiveCountsDTO)
        {
            _dbcontext = dbcontext;
            _ClientActiveCountsDTO = ClientActiveCountsDTO;

        }


        public List<ClientActiveCountsDTO> getclientstatusfordashboard(string opcode)
        {

            List<ClientActiveCountsDTO> ListClientcount = new List<ClientActiveCountsDTO>();
            try
            {
                string procName = "USP_GetGlobalDataByCategoryIds";
                var ParamsArray = new SqlParameter[5];
                ParamsArray[0] = new SqlParameter() { ParameterName = "@pGlobalCodeCategoryValue", Value = "", DbType = System.Data.DbType.String };
                ParamsArray[1] = new SqlParameter() { ParameterName = "@pOpCode", Value = opcode, DbType = System.Data.DbType.String };
                ParamsArray[2] = new SqlParameter() { ParameterName = "@pOpParams", Value = "", DbType = System.Data.DbType.String };
                ParamsArray[3] = new SqlParameter() { ParameterName = "@pLoggedInUser", Value = "", DbType = System.Data.DbType.String };
                ParamsArray[4] = new SqlParameter() { ParameterName = "@pLNCode", Value = "", DbType = System.Data.DbType.String };

                var resultData = _ClientActiveCountsDTO.ExecuteWithJsonResult(procName, "Statuscount", ParamsArray);

                if (resultData != null)
                {
                    return resultData.ToList();
                }
                else
                {
                    return ListClientcount;
                }
            }
            catch (ApplicationException ex)
            {
                throw;
            }




        }
    }
}
