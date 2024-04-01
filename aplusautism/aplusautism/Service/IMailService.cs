using aplusautism.Models;

namespace aplusautism.Service
{
    public interface IMailService
    {
        string SendEmailAsync(string mailRequest, string Password, string Firstname, string emailBody);

        string SendPaymentEmail(string Sendto, string EmailMessage);

        string SendReminderEmail(string Sendto, string EmailMessage);

        string SendSuspendEmail(string Sendto, string EmailMessage);

        string SendContactUsEmail(string Sendto, string EmailMessage, string Subject);

        string SendRegisterEmail(string Sendto, string EmailMessage);

        string SendForgotPassword(string Sendto, string EmailMessage);


        Task SendmailTest(string From, dynamic To, string subject, string plainTextContent, string htmlContent);



    }
}
