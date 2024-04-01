using aplusautism.Bal.DTO;
using aplusautism.Bal.ILogic;
using aplusautism.Common.Enums;
using aplusautism.Data;
using aplusautism.Data.Models;
using aplusautism.Repository.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Bal.Logic
{
    public class SubscriptionsetupLogic : ISubscriptionsetup
    {

        private readonly IRepository<SubscriptionSetup> _SubscriptionSetup;

        public  AplusautismDbContext _dbcontext;

        public readonly IRepository<SubscriptionDTO> _SubscriptionDTO;
        private readonly IRepository<UserDTO> _UserDTO;

        public SubscriptionsetupLogic(IRepository<SubscriptionSetup> SubscriptionSetup, AplusautismDbContext dbcontext,
            IRepository<SubscriptionDTO> subscriptionDTO, IRepository<UserDTO> UserDTO)
        {
            _SubscriptionSetup = SubscriptionSetup;
            _dbcontext = dbcontext;
            _SubscriptionDTO = subscriptionDTO;
            _UserDTO = UserDTO;
        }

        public UserDTO SaveDeviceTracking(DeviceTrackingDTO deviceTrackingDTO)
        {
            try
            {

                string procName = SPROC_Names.SpSaveDeviceTracking.ToString();
                var ParamsArray = new SqlParameter[6];
                ParamsArray[0] = new SqlParameter() { ParameterName = "@DeviceId", Value = deviceTrackingDTO.DeviceId, DbType = System.Data.DbType.Int64 };
                ParamsArray[1] = new SqlParameter() { ParameterName = "@DeviceName", Value = deviceTrackingDTO.DeviceName, DbType = System.Data.DbType.String };
                ParamsArray[2] = new SqlParameter() { ParameterName = "@UserName", Value = deviceTrackingDTO.UserName, DbType = System.Data.DbType.String };
                ParamsArray[3] = new SqlParameter() { ParameterName = "@UserId", Value = deviceTrackingDTO.UserId, DbType = System.Data.DbType.String };
                ParamsArray[4] = new SqlParameter() { ParameterName = "@CreatedBy", Value = deviceTrackingDTO.CreatedBy, DbType = System.Data.DbType.String };
                ParamsArray[5] = new SqlParameter() { ParameterName = "@IpAddress", Value = deviceTrackingDTO.IpAddress, DbType = System.Data.DbType.String };

                var resultData = _UserDTO.ExecuteWithJsonResult(procName, "UserDTO", ParamsArray);
                return resultData != null ? resultData.FirstOrDefault() : resultData.FirstOrDefault();
            }
            catch (ApplicationException ex)
            {
                throw;
            }
        }

        public List<SubscriptionSetup> GetSubscriptionData()
        {

            try
            {
                List<SubscriptionSetup> NewList = new List<SubscriptionSetup>();
                var subcriptionList = _dbcontext.SubscriptionSetup.Where(i => i.IsDeleted != true);
                if (subcriptionList != null)
                {
                    NewList = subcriptionList.ToList();

                    foreach (var listdata in NewList)
                    {
                         //if (listdata.SchemeName == "Gold")
                        if (listdata.SchemeName == "Gold Plan")
                        {
                            listdata.CreatedBy = 3;
                        }
                        //if (listdata.SchemeName == "Silver")
                        if (listdata.SchemeName == "Silver Plan")
                        {
                            listdata.CreatedBy = 2;
                        }
                        //if (listdata.SchemeName == "Bronze")
                        if (listdata.SchemeName == "Bronze Plan")
                        {
                            listdata.CreatedBy = 1;
                        }
                    }
                   
                }
                NewList = NewList.OrderByDescending(i => i.CreatedBy).ToList();
                return NewList;
            }
            catch (ApplicationException ex)
            {
                return null;
            }
        }

        public SubscriptionSetup UpdateSubscriptionData(UpdateSubscriptionPlanDTO updatedata)
        {

            var subcriptionListManyValues = _SubscriptionSetup.Where(i => i.SubscriptionSetupID == updatedata.planid).FirstOrDefault();

            var subcriptionList = _SubscriptionSetup.Where(i => i.SubscriptionSetupID == updatedata.planid).FirstOrDefault();
           
            subcriptionList.IsDeleted = true;
            _dbcontext.SubscriptionSetup.Update(subcriptionList);
            _dbcontext.SaveChanges();


            //inserting new plan
            SubscriptionSetup newplanobj = new SubscriptionSetup();

            newplanobj.Duration = Convert.ToInt32(updatedata.monthdata);
            newplanobj.Price = Convert.ToInt32(updatedata.pricedata);
            newplanobj.ActivationStart = DateTime.Now;
            newplanobj.DurationType = subcriptionList.DurationType;
            newplanobj.ActivationEnd = DateTime.Now.AddYears(1);
            newplanobj.SubscriptionImage = subcriptionListManyValues.SubscriptionImage;
            newplanobj.SchemeName = subcriptionListManyValues.SchemeName;
           

            _dbcontext.SubscriptionSetup.Add(newplanobj);
            _dbcontext.SaveChanges();
            return newplanobj;
        }

        public SubscriptionSetup EditSubscriptionData(int id)
        {
            var subcriptionList = _SubscriptionSetup.Where(i=>i.SubscriptionSetupID==id).FirstOrDefault();

            return subcriptionList;
        }

        public List<SubscriptionDTO> GetLatestSubscriptionData(string opcode)
        {
            List<SubscriptionDTO> Listdata = new List<SubscriptionDTO>();
            try
            {
                string procName = SPROC_Names.USP_GetLatestSubscriptionData.ToString();
                var ParamsArray = new SqlParameter[1];
                ParamsArray[0] = new SqlParameter() { ParameterName = "@opcode", Value = opcode, DbType = System.Data.DbType.String };
                var resultData = _SubscriptionDTO.ExecuteWithJsonResult(procName, "SubscriptionDTO", ParamsArray);

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
    }
}
