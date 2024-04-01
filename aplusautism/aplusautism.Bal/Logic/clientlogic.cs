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
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Bal.Logic
{
    public class clientlogic: Iclientlogic
    {

        private readonly IRepository<AB_User> _clientlogic;
     

        private readonly IRepository<ABuserDTO> _ABuserDTO;
        public AplusautismDbContext _dbcontext;

        public clientlogic(IRepository<AB_User> clientlogic, AplusautismDbContext dbcontext, IRepository<ABuserDTO> ABuserDTO)
        {
            _clientlogic = clientlogic;
            _dbcontext = dbcontext;
            _ABuserDTO = ABuserDTO;
        }

        public string DeleteClient(int ClientId)
        {
            try
            {
                // For UPdate Old status values 

                var olddata = _dbcontext.aB_User.Where(i => i.AB_MainID == ClientId).FirstOrDefault();
                olddata.oldstatus = olddata.Status;
                _dbcontext.SaveChanges();

                var Newdata = _dbcontext.aB_User.Where(i => i.AB_MainID == ClientId).FirstOrDefault();
                Newdata.Status = "2";
                _dbcontext.SaveChanges();

                var getdata = _dbcontext.Ab_main.Where(i => i.AB_MainID == ClientId).FirstOrDefault();
                getdata.IsDeleted = true;

                _dbcontext.SaveChanges();


                //ab user update
                var AbuseruPDATE = _dbcontext.aB_User.Where(v => v.AB_MainID == ClientId).FirstOrDefault();
                AbuseruPDATE.IsDeleted = true;

                _dbcontext.SaveChanges();

                return "success";
            }

            catch(ApplicationException ex)
            {
                return "fail";
            }
        }

        public List<ABuserDTO> GetClientDetail(int UserId)
        {
            try
            {
                string procName = SPROC_Names.USP_GetClientDetail.ToString();
                var ParamsArray = new SqlParameter[1];
                ParamsArray[0] = new SqlParameter() { ParameterName = "@UserId", Value = UserId, DbType = System.Data.DbType.Int32 };
                var resultData = _ABuserDTO.ExecuteWithJsonResult(procName, "AB_User", ParamsArray);

                if (resultData != null)
                {
                    return resultData.ToList();
                }
                else
                {
                    return resultData;
                }
            }
            catch (ApplicationException ex)
            {
                throw;
            }
        }
        public List<ABuserDTO> GetClientList(string opcode ,string pOpParams)
        {
            try
            {
                string procName = SPROC_Names.USP_GetClientDetails.ToString();
                var ParamsArray = new SqlParameter[5];
                ParamsArray[0] = new SqlParameter() { ParameterName = "@pGlobalCodeCategoryValue", Value = "", DbType = System.Data.DbType.String };
                ParamsArray[1] = new SqlParameter() { ParameterName = "@pOpCode", Value = opcode, DbType = System.Data.DbType.String };
                ParamsArray[2] = new SqlParameter() { ParameterName = "@pOpParams", Value = pOpParams, DbType = System.Data.DbType.String };
                ParamsArray[3] = new SqlParameter() { ParameterName = "@pLoggedInUser", Value = "", DbType = System.Data.DbType.String };
                ParamsArray[4] = new SqlParameter() { ParameterName = "@pLNCode", Value = "", DbType = System.Data.DbType.String };

                var resultData = _ABuserDTO.ExecuteWithJsonResult(procName, "clientdetails", ParamsArray);

                if (resultData != null)
                {
                    return resultData.ToList();
                }
                else
                {
                    return resultData;
                }
            }
            catch (ApplicationException ex)
            {
                throw;
            }
        }
    }
}
