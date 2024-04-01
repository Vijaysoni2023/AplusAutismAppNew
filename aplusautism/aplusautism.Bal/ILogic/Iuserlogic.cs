using aplusautism.Bal.DTO;
using aplusautism.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace aplusautism.Bal.ILogic
{
    public interface Iuserlogic
    {
        UserDTO GetLoginUserDetails(string email, string opcode);
        List<UserDTO> GetUserDataList();

        void Insertsubsription(SubscriptionSetup setup);
        string RegisterUser(RegisterUserDTO User);
        AB_User LoginCheck(LoginDTO loginDTO);
        AB_User ChangePwd(LoginDTO loginDTO);
        List<ForgetPasswordDTO> ForgetPassword(string usermail);

        // for Update API Response
        PaymentsDTO UpdateStripeAPIResponse(string PaymentID, string APIResponse, bool PaymentStatus);


        List<AB_User> CheckForUser(string Email, string Password);



        // function for getting Payments
        List<PaymentsDTO> GetPaymentsDetails(string Opcode, DateTime StartDate, DateTime EndDate);
        PaymentsDTO UpdateClientStatus(string AbUserId, string AbUserMainId );

        UserDTO SaveContactLog(ContactLogDTO contactLogDTO);

        List<AB_Main> GetUserLangauge(string Email, string Password);
        //ABuserDTO GetUserLangauge(string Email, string Password);

        List<AB_Main> GetUserDeviceIdCount(int UserId, string DeviceId);

        List<PaymentsDTO> GetPLanName(int SubscriptionSetupID);


        
    }
}