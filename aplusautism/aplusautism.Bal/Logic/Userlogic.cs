using aplusautism.Bal.DTO;
using aplusautism.Bal.ILogic;
using aplusautism.Common;
using aplusautism.Common.Enums;
using aplusautism.Data.Models;
using aplusautism.Repository.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Bal.Logic
{
    public class Userlogic : Iuserlogic
    {
        private readonly IRepository<PaymentsDTO> _PaymentsDTO;
        private readonly IRepository<UserDTO> _UserDTO;
        private readonly IRepository<ForgetPasswordDTO> _ForgetPasswordDTO;
        private readonly IRepository<SubscriptionSetup> _SubscriptionSetup;
        private readonly IRepository<AB_Address> _Address;
        private readonly IRepository<AB_User> _user;
        private readonly IRepository<LessonSetupLanguage> _Language;
        private readonly IRepository<AB_Main> _AbMain;
        private readonly IRepository<LessonSetup> _LessonSetup;
        private readonly IRepository<GlobalCodeCategory> _globalCodeCategory;
        private readonly IRepository<GlobalCodes> _globalCode;
        private readonly IRepository<ABuserDTO> _ABuserDTO;
        //constructor
        public Userlogic(IRepository<UserDTO> UserDTO, IRepository<SubscriptionSetup> subscriptionSetup, IRepository<AB_Address> Ab_Address,
            IRepository<AB_User> Ab_User, IRepository<LessonSetupLanguage> LessonLanguage, IRepository<AB_Main> AbMain, IRepository<GlobalCodeCategory> globalCategory,
            IRepository<GlobalCodes> globalCode, IRepository<ForgetPasswordDTO> forgetPasswordDTO, IRepository<PaymentsDTO> paymentsDTO, IRepository<ABuserDTO> ABuserDTO)
        {
            _UserDTO = UserDTO;
            _SubscriptionSetup = subscriptionSetup;
            _Address = Ab_Address;
            _user = Ab_User;
            _Language = LessonLanguage;
            _AbMain = AbMain;
            _globalCodeCategory = globalCategory;
            _globalCode = globalCode;
            _ForgetPasswordDTO = forgetPasswordDTO;
            _PaymentsDTO = paymentsDTO; 
            ABuserDTO = _ABuserDTO;
        }
        //calling store proc
        public UserDTO GetLoginUserDetails(string email,string opcode)
        {
            try
            {
                string procName = SPROC_Names.sp_getusers.ToString();
                var ParamsArray = new SqlParameter[2];
                ParamsArray[0] = new SqlParameter() { ParameterName = "@OpCode", Value = opcode, DbType = System.Data.DbType.String };
                ParamsArray[1] = new SqlParameter() { ParameterName = "@Email", Value = email, DbType = System.Data.DbType.String };
                var resultData = _UserDTO.ExecuteWithJsonResult(procName, "Userdto", ParamsArray);

                return resultData != null ? resultData.FirstOrDefault() : resultData.FirstOrDefault();
            }
            catch (ApplicationException ex)
            {
                throw;
            }
        }
        //we will apply
        public List<UserDTO> GetUserDataList()
        {
            throw new NotImplementedException();
        }
        public void Insertsubsription(SubscriptionSetup setup)
        {
            _SubscriptionSetup.Insert(setup);
        }
        public string RegisterUser(RegisterUserDTO User)
        {
            try
            {
                int StatusValue = Convert.ToInt32(Status.Trial);
                AB_Main UserMain = new AB_Main
                {
                    Email = User.Email.Trim(),
                    AB_Type = CommonFunction.ABMainTypeAD,
                    FirstName = User.FirstName.Trim(),
                    LastName = User.LastName.Trim(),
                    Status = StatusValue.ToString(),
                    CreatedBy = 1,
                    CreatedDate = CommonFunction.CurrentDate,
                    IsDeleted = false,
                    ContactNumber= User.ContactNumber,
                    PreferedLanguage =Convert.ToInt32(User.Languages),
                };
                _AbMain.Insert(UserMain);
                var AB_MainId = UserMain.AB_MainID;

                AB_Address SubsList = new AB_Address
                {
                    //AB_AddressID = 1,
                    AddressType = AddressType.BillingAddress.ToString(),
                    AB_MainID = AB_MainId,
                    IsPrimary = true,
                    CreatedBy = 1,
                    CreatedDate = CommonFunction.CurrentDate,
                    IsDeleted = false,
                    Address1 = "None",// User.PermanentAddress.Trim(),
                    Address2 = "None",// User.CurrentAddress.Trim(),
                    CountryId = Convert.ToInt32(User.Country),
                    StateID = "0", //User.State,
                    CityID = "0"//User.City,


                };
                _Address.Insert(SubsList);


              
                AB_User user = new AB_User
                {
                    AB_MainID = AB_MainId,
                    Status = StatusValue.ToString(),//todo: add in enum
                    CreatedDate = CommonFunction.CurrentDate,
                    Password = User.Password.Trim(),
                    UserName = User.Email.Trim(),
                    UserType="client"

                };
                _user.Insert(user);


                LessonSetupLanguage language = new LessonSetupLanguage
                {
                    CreatedDate = CommonFunction.CurrentDate,
                    LanguageID = User.Languages

                };
                _Language.Insert(language);

                return "Success";
            }

            catch (ApplicationException ex)
            {
                return "Faild";
            }
        }
        public AB_User LoginCheck(LoginDTO loginDTO)
        {
            try
            {
                string procName = SPROC_Names.USP_GetGlobalDataByCategoryIds.ToString();
                var ParamsArray = new SqlParameter[5];

                ParamsArray[0] = new SqlParameter() { ParameterName = "@pGlobalCodeCategoryValue", Value = "1", DbType = System.Data.DbType.String };
                ParamsArray[1] = new SqlParameter() { ParameterName = "@pOpCode", Value = 1, DbType = System.Data.DbType.String };
                ParamsArray[2] = new SqlParameter() { ParameterName = "@pOpParams", Value = loginDTO.Email, DbType = System.Data.DbType.String };
                ParamsArray[3] = new SqlParameter() { ParameterName = "@pLoggedInUser", Value = 0, DbType = System.Data.DbType.String };
                ParamsArray[4] = new SqlParameter() { ParameterName = "@pLNCode", Value = loginDTO.Password, DbType = System.Data.DbType.String };
                var resultDataDB = _user.ExecuteWithJsonResult(procName, "AB_User", ParamsArray);
                AB_User resultData = null;
                if (resultDataDB != null)
                {
                    resultData = resultDataDB.FirstOrDefault();
                    if (resultData == null)
                    {
                        return resultData;
                    }
                    return resultData;

                } else
                {
                    
                }

                return resultData;

            }

            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        public AB_User ChangePwd (LoginDTO loginDTO)
        {
            AB_User objdefault = new AB_User();
            try
            {
                //TODO:-THIS  SHOULD BE UPDATE FROM THE ENTITY
                string procName = SPROC_Names.USP_GetGlobalDataByCategoryIds.ToString();
                var ParamsArray = new SqlParameter[5];

                ParamsArray[0] = new SqlParameter() { ParameterName = "@pGlobalCodeCategoryValue", Value = loginDTO.Email, DbType = System.Data.DbType.String };
                ParamsArray[1] = new SqlParameter() { ParameterName = "@pOpCode", Value = 7, DbType = System.Data.DbType.String };
                ParamsArray[2] = new SqlParameter() { ParameterName = "@pOpParams", Value = loginDTO.NewPassword, DbType = System.Data.DbType.String };
                ParamsArray[3] = new SqlParameter() { ParameterName = "@pLoggedInUser", Value = 0, DbType = System.Data.DbType.String };
                ParamsArray[4] = new SqlParameter() { ParameterName = "@pLNCode", Value = "0", DbType = System.Data.DbType.String };
                var resultData = _user.ExecuteWithJsonResult(procName, "AB_User", ParamsArray);

                if(resultData!=null)
                {
                    resultData = resultData.ToList();
                }
                if (resultData == null)
                {
                    return objdefault;

                    
                }
                return resultData.FirstOrDefault();
            }

            catch (ApplicationException ex)
            {
                throw ex;
            }
        }


        ////calling store proc
        //public ABuserDTO GetUserLangauge(string Email, string Password)
        //{
        //    try
        //    {
        //        string procName = SPROC_Names.sp_getusersPreferedLanguage.ToString();
        //        var ParamsArray = new SqlParameter[2];
        //        ParamsArray[0] = new SqlParameter() { ParameterName = "@Email", Value = Email, DbType = System.Data.DbType.String };
        //        ParamsArray[1] = new SqlParameter() { ParameterName = "@Password", Value = Password, DbType = System.Data.DbType.String };
        //        var resultData = _ABuserDTO.ExecuteWithJsonResult(procName, "Userdto", ParamsArray);

        //        return resultData != null ? resultData.FirstOrDefault() : resultData.FirstOrDefault();
        //    }
        //    catch (ApplicationException ex)
        //    {
        //        throw;
        //    }
        //}

        public List<AB_Main> GetUserLangauge(string Email, string Password)
        {
            try
            {
                string procName = SPROC_Names.sp_getusersPreferedLanguage.ToString();
                var ParamsArray = new SqlParameter[2];
                ParamsArray[0] = new SqlParameter() { ParameterName = "@Email", Value = Email, DbType = System.Data.DbType.String };
                ParamsArray[1] = new SqlParameter() { ParameterName = "@Password", Value = Password, DbType = System.Data.DbType.String };
                var resultData = _AbMain.ExecuteWithJsonResult(procName, "AB_User", ParamsArray);
                return resultData != null ? resultData.ToList() : new List<AB_Main>();
            }
            catch (ApplicationException ex)
            {
                throw;
            }
        }

        public List<AB_Main> GetUserDeviceIdCount(int UserId , string DeviceId)
        {
            try
            {
                string procName = SPROC_Names.sp_GetUserDeviceIdCount.ToString();
                var ParamsArray = new SqlParameter[2];
                ParamsArray[0] = new SqlParameter() { ParameterName = "@UserId", Value = UserId, DbType = System.Data.DbType.String };
                ParamsArray[1] = new SqlParameter() { ParameterName = "@DeviceId", Value = DeviceId, DbType = System.Data.DbType.String };
                var resultData = _AbMain.ExecuteWithJsonResult(procName, "AB_User", ParamsArray);
                return resultData != null ? resultData.ToList() : new List<AB_Main>();
            }
            catch (ApplicationException ex)
            {
                throw;
            }
        }

        public List<AB_User> CheckForUser(string Email, string Password)
        {
            try
            {
                string procName = SPROC_Names.USP_GetGlobalDataByCategoryIds.ToString();
                var ParamsArray = new SqlParameter[5];
                ParamsArray[0] = new SqlParameter() { ParameterName = "@pGlobalCodeCategoryValue", Value = "1", DbType = System.Data.DbType.String };
                ParamsArray[1] = new SqlParameter() { ParameterName = "@pOpCode", Value = 1, DbType = System.Data.DbType.String };
                ParamsArray[2] = new SqlParameter() { ParameterName = "@pOpParams", Value = Email, DbType = System.Data.DbType.String };
                ParamsArray[3] = new SqlParameter() { ParameterName = "@pLoggedInUser", Value = 0, DbType = System.Data.DbType.String };
                ParamsArray[4] = new SqlParameter() { ParameterName = "@pLNCode", Value = Password, DbType = System.Data.DbType.String };
                var resultData = _user.ExecuteWithJsonResult(procName, "AB_User", ParamsArray);
                return resultData != null ? resultData.ToList() : new List<AB_User>();
            }
            catch (ApplicationException ex)
            {
                throw;
            }
        }

       

     

        // function for Update Payment values in Payment table.
        public PaymentsDTO UpdateStripeAPIResponse(string PaymentID, string APIResponse, bool PaymentStatus)
        {
            try
            {
                string procName = SPROC_Names.SpUpdateStripeAPIResponse.ToString();
                var ParamsArray = new SqlParameter[3];
                ParamsArray[0] = new SqlParameter() { ParameterName = "@PaymentID", Value = PaymentID, DbType = System.Data.DbType.String };
                ParamsArray[1] = new SqlParameter() { ParameterName = "@APIResponse", Value = APIResponse, DbType = System.Data.DbType.String };
                ParamsArray[2] = new SqlParameter() { ParameterName = "@PaymentStatus", Value = PaymentStatus, DbType = System.Data.DbType.String };
                var resultData = _PaymentsDTO.ExecuteWithJsonResult(procName, "PaymentsDTO", ParamsArray);
                return resultData != null ? resultData.FirstOrDefault() : resultData.FirstOrDefault();
            }
            catch (ApplicationException)
            {
                throw;
            }
        }

        public UserDTO SaveContactLog(ContactLogDTO contactLogDTO)
        {
            try
            {

                string procName = SPROC_Names.SpSaveContactLog.ToString();
                var ParamsArray = new SqlParameter[7];
                ParamsArray[0] = new SqlParameter() { ParameterName = "@AB_MAINID", Value = contactLogDTO.AB_MAINID, DbType = System.Data.DbType.Int64 };
                ParamsArray[1] = new SqlParameter() { ParameterName = "@ContactType", Value = contactLogDTO.ContactType, DbType = System.Data.DbType.String };
                ParamsArray[2] = new SqlParameter() { ParameterName = "@ContactTypeTopic", Value = contactLogDTO.ContactTypeTopic, DbType = System.Data.DbType.String };
                ParamsArray[3] = new SqlParameter() { ParameterName = "@ContactMethod", Value = contactLogDTO.ContactMethod, DbType = System.Data.DbType.String };               
                ParamsArray[4] = new SqlParameter() { ParameterName = "@Subject", Value = contactLogDTO.Subject, DbType = System.Data.DbType.String };
                ParamsArray[5] = new SqlParameter() { ParameterName = "@Disposition", Value = contactLogDTO.Disposition, DbType = System.Data.DbType.String };            
                ParamsArray[6] = new SqlParameter() { ParameterName = "@CreatedBy", Value = contactLogDTO.CreatedBy, DbType = System.Data.DbType.String };
               
                var resultData = _UserDTO.ExecuteWithJsonResult(procName, "UserDTO", ParamsArray);
                return resultData != null ? resultData.FirstOrDefault() : resultData.FirstOrDefault();
            }
            catch (ApplicationException ex)
            {
                throw;
            }
        }
        public PaymentsDTO UpdateClientStatus(string AbUserId, string AbUserMainId)
        {
            try
            {
                string procName = SPROC_Names.SpUpdateClientStatus.ToString();
                var ParamsArray = new SqlParameter[2];
                ParamsArray[0] = new SqlParameter() { ParameterName = "@AbUserId", Value = AbUserId, DbType = System.Data.DbType.String };
                ParamsArray[1] = new SqlParameter() { ParameterName = "@AbUserMainId", Value = AbUserMainId, DbType = System.Data.DbType.String };
                var resultData = _PaymentsDTO.ExecuteWithJsonResult(procName, "PaymentsDTO", ParamsArray);
                return resultData != null ? resultData.FirstOrDefault() : resultData.FirstOrDefault();
            }
            catch (ApplicationException)
            {
                throw;
            }
        }
        public List<ForgetPasswordDTO> ForgetPassword(string UserName)
        {
            try
            {
                string procName = SPROC_Names.USP_GetGlobalDataByCategoryIds.ToString();
                var ParamsArray = new SqlParameter[5];

                ParamsArray[0] = new SqlParameter() { ParameterName = "@pGlobalCodeCategoryValue", Value = "1", DbType = System.Data.DbType.String };
                ParamsArray[1] = new SqlParameter() { ParameterName = "@pOpCode", Value = 3, DbType = System.Data.DbType.String };
                ParamsArray[2] = new SqlParameter() { ParameterName = "@pOpParams", Value = UserName, DbType = System.Data.DbType.String };
                ParamsArray[3] = new SqlParameter() { ParameterName = "@pLoggedInUser", Value = 0, DbType = System.Data.DbType.String };
                ParamsArray[4] = new SqlParameter() { ParameterName = "@pLNCode", Value = "", DbType = System.Data.DbType.String };
                var resultData = _ForgetPasswordDTO.ExecuteWithJsonResult(procName, "ForgetPasswordDTO", ParamsArray);
                if (resultData == null)
                {
                    List<ForgetPasswordDTO> Emptylist = new List<ForgetPasswordDTO>();
                    return Emptylist;
                }
                return resultData;
            }

            catch (ApplicationException ex)
            {
                throw ex;
            }
        }

        public List<PaymentsDTO> GetPaymentsDetails(string Opcode , DateTime StartDate, DateTime EndDate)
        {
            List<PaymentsDTO> Listdata = new List<PaymentsDTO>();
            try
            {
                string procName = SPROC_Names.USP_GetPaymentDetails.ToString();
                var ParamsArray = new SqlParameter[3];
                ParamsArray[0] = new SqlParameter() { ParameterName = "@opcode", Value = Opcode, DbType = System.Data.DbType.String };
                ParamsArray[1] = new SqlParameter() { ParameterName = "@StartDate", Value = StartDate, DbType = System.Data.DbType.DateTime };
                ParamsArray[2] = new SqlParameter() { ParameterName = "@EndDate", Value = EndDate, DbType = System.Data.DbType.DateTime };
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

        public List<PaymentsDTO> GetPLanName(int SubscriptionSetupID)
        {
            List<PaymentsDTO> Listdata = new List<PaymentsDTO>();
            try
            {
                string procName = SPROC_Names.USP_GetPlanName.ToString();
                var ParamsArray = new SqlParameter[1];
                ParamsArray[0] = new SqlParameter() { ParameterName = "@SubscriptionSetupID", Value = SubscriptionSetupID, DbType = System.Data.DbType.String };               
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
