using aplusautism.Areas.Admin.Models;
using aplusautism.Models;
using aplusautism.Setting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.Web.Administration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Configuration;
using System.Net;
using System.Net.Mail;



namespace aplusautism.Service
{
    public class MailService : IMailService
    {
        private readonly MailSetting _mailSetting;
        private readonly IConfiguration _configuration;
        public MailService(IOptions<MailSetting> mailSetting, IConfiguration configuration)
        {
            _mailSetting = mailSetting.Value;
            _configuration = configuration;
        }
        public string SendEmailAsync(string mailRequest, string Password, string Firstname,string emailBody)
        {

            using (SmtpClient smtpClient = new SmtpClient())
            {
                var basicCredential = new NetworkCredential(_mailSetting.Mail, _mailSetting.Password);
                using (MailMessage message = new MailMessage())
                {
                    MailAddress fromAddress = new MailAddress(_mailSetting.Mail);

                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = basicCredential;
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    message.From = fromAddress;
                    message.Subject = "Password Mail";
                    //Set IsBodyHtml to true means you can send HTML email.
                    message.IsBodyHtml = true;
                    message.Body = emailBody;//"Hello " + Firstname + " </br> </br> This Is Your Email " + mailRequest + " and  your credentials is given as below : </br></br> User Name : "+ mailRequest + " </br> Password : " + Password + "  </br> </br> Thanks";
                    message.To.Add(mailRequest);

                    //message.To.Add("galenec444@ketchet.com");

                    try
                    {
                        smtpClient.Send(message);
                        return "Success";
                    }
                    catch (ApplicationException ex)
                    {
                        return "Faild";
                        //Error, could not send the message
                    }
                }

            }
        }


        public string SendReminderEmail(string Sendto, string EmailMessage)
        {

            using (SmtpClient smtpClient = new SmtpClient())
            {
                var basicCredential = new NetworkCredential(_mailSetting.Mail, _mailSetting.Password);
                using (MailMessage message = new MailMessage())
                {
                    MailAddress fromAddress = new MailAddress(_mailSetting.Mail);

                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = basicCredential;
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    message.From = fromAddress;
                    message.Subject = "Reminder Notifcation";
                    //Set IsBodyHtml to true means you can send HTML email.
                    message.IsBodyHtml = true;
                    message.Body = EmailMessage;
                    message.To.Add(Sendto);

                    //message.To.Add("galenec444@ketchet.com");

                    try
                    {
                        smtpClient.Send(message);
                        return "Success";
                    }
                    catch (ApplicationException ex)
                    {
                        return "Faild";
                        //Error, could not send the message
                    }
                }

            }
        }

        public string SendRegisterEmail(string Sendto, string EmailMessage)
        {

            using (SmtpClient smtpClient = new SmtpClient())
            {
                var basicCredential = new NetworkCredential(_mailSetting.Mail, _mailSetting.Password);
                using (MailMessage message = new MailMessage())
                {
                    MailAddress fromAddress = new MailAddress(_mailSetting.Mail);

                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = basicCredential;
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    message.From = fromAddress;
                    message.Subject = "Register Notification";
                    //Set IsBodyHtml to true means you can send HTML email.
                    message.IsBodyHtml = true;
                    message.Body = EmailMessage;
                    message.To.Add(Sendto);

                    //message.To.Add("galenec444@ketchet.com");

                    try
                    {
                        smtpClient.Send(message);
                        return "Success";
                    }
                    catch (ApplicationException ex)
                    {
                        return "Faild";
                        //Error, could not send the message
                    }
                }

            }
        }
        public async Task<IActionResult> SendgridEmailSubmit(Emailmodel emailmodel)
        {
           // ViewData["Message"] = "Email Sent!!!...";
            Example emailexample = new Example();
            await emailexample.Execute(emailmodel.From, emailmodel.To, emailmodel.Subject, emailmodel.Body
                , emailmodel.Body);

            return null;
        }
        internal class Example
        {
            public async Task Execute(string From, string To, string subject, string plainTextContent, string htmlContent)
            {
                //   var apiKey = "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";
                var apiKey = "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";
                var client = new SendGridClient(apiKey);
                var from = new EmailAddress("sharmamahesh2324@gmail.com");
                var to = new EmailAddress("usert888a@gmail.com");
                htmlContent = "<strong>" + htmlContent + "</strong>";
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
            }
        }

        public string SendSuspendEmail(string Sendto, string EmailMessage)
        {

            using (SmtpClient smtpClient = new SmtpClient())
            {
                var basicCredential = new NetworkCredential(_mailSetting.Mail, _mailSetting.Password);
                using (MailMessage message = new MailMessage())
                {
                    MailAddress fromAddress = new MailAddress(_mailSetting.Mail);

                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = basicCredential;
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    message.From = fromAddress;
                    message.Subject = "Notifcation";
                    //Set IsBodyHtml to true means you can send HTML email.
                    message.IsBodyHtml = true;
                    message.Body = EmailMessage;
                    message.To.Add(Sendto);

                    //message.To.Add("galenec444@ketchet.com");

                    try
                    {
                        smtpClient.Send(message);
                        return "Success";
                    }
                    catch (ApplicationException ex)
                    {
                        return "Faild";
                        //Error, could not send the message
                    }
                }

            }
        }

        public string SendContactUsEmail(string Sendto, string EmailMessage , string Subject)
        {

            using (SmtpClient smtpClient = new SmtpClient())
            {
                var basicCredential = new NetworkCredential(_mailSetting.Mail, _mailSetting.Password);
                using (MailMessage message = new MailMessage())
                {
                    MailAddress fromAddress = new MailAddress(_mailSetting.Mail);

                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = basicCredential;
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    message.From = fromAddress;
                    message.Subject = Subject;
                    //Set IsBodyHtml to true means you can send HTML email.
                    message.IsBodyHtml = true;
                    message.Body = EmailMessage;
                    message.To.Add(Sendto);

                    //message.To.Add("galenec444@ketchet.com");

                    try
                    {
                        smtpClient.Send(message);
                        return "Success";
                    }
                    catch (ApplicationException ex)
                    {
                        return "Faild";
                        //Error, could not send the message
                    }
                }

            }
        }


        //public async Task<string> SendEmailBySendGridAsync()
        //{
        //    var apiKey = Environment.GetEnvironmentVariable("NAME_OF_THE_ENVIRONMENT_VARIABLE_FOR_YOUR_SENDGRID_KEY");
        //    var client = new SendGridClient(apiKey);
        //    var from = new EmailAddress("test@example.com", "Example User");
        //    var subject = "Sending with SendGrid is Fun";
        //    var to = new EmailAddress("test@example.com", "Example User");
        //    var plainTextContent = "and easy to do anywhere, even with C#";
        //    var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
        //    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        //    var response = await client.SendEmailAsync(msg);
        //}
        //public IActionResult SendgridEmail()
        //{
        //   // ViewData["Message"] = "Your application description page.";

        //    return View();
        //}
        [HttpPost]
        //public async Task<IActionResult> SendgridEmailSubmit(Emailmodel emailmodel)
        //{
        //    //ViewData["Message"] = "Email Sent!!!...";
        //    Example emailexample = new Example();
        //    await emailexample.Execute(emailmodel.From, emailmodel.To, emailmodel.Subject, emailmodel.Body
        //        , emailmodel.Body);

        //    return View("SendgridEmail");
        //}

        //internal class Example
        //{
        //    public async Task Execute(string From, string To, string subject, string plainTextContent, string htmlContent)
        //    {
        //        var apiKey = "your send grid api goes here!!..";
        //        var client = new SendGridClient(apiKey);
        //        var from = new EmailAddress(From);
        //        var to = new EmailAddress(To);
        //        htmlContent = "<strong>" + htmlContent + "</strong>";
        //        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        //        var response = await client.SendEmailAsync(msg);
        //    }
        //}
        public string SendPaymentEmail(string Sendto, string EmailMessage)
        {

            using (SmtpClient smtpClient = new SmtpClient())
            {
                var basicCredential = new NetworkCredential(_mailSetting.Mail, _mailSetting.Password);
                using (MailMessage message = new MailMessage())
                {
                    MailAddress fromAddress = new MailAddress(_mailSetting.Mail);

                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = basicCredential;
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    message.From = fromAddress;
                    message.Subject = "Payment Notification";
                    //Set IsBodyHtml to true means you can send HTML email.
                    message.IsBodyHtml = true;
                    message.Body = EmailMessage;
                    message.To.Add(Sendto);

                    //message.To.Add("galenec444@ketchet.com");

                    try
                    {
                        smtpClient.Send(message);
                        return "Success";
                    }
                    catch (ApplicationException ex)
                    {
                        return "Faild";
                        //Error, could not send the message
                    }
                }

            }
        }

        public string SendForgotPassword(string Sendto, string EmailMessage)
        {

            using (SmtpClient smtpClient = new SmtpClient())
            {
                var basicCredential = new NetworkCredential(_mailSetting.Mail, _mailSetting.Password);
                using (MailMessage message = new MailMessage())
                {
                    MailAddress fromAddress = new MailAddress(_mailSetting.Mail);

                    smtpClient.Host = "smtp.gmail.com";
                    smtpClient.Port = 587;
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.Credentials = basicCredential;
                    smtpClient.EnableSsl = true;
                    smtpClient.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                    message.From = fromAddress;
                    message.Subject = "Password Notification";
                    //Set IsBodyHtml to true means you can send HTML email.
                    message.IsBodyHtml = true;
                    message.Body = EmailMessage;
                    message.To.Add(Sendto);

                    //message.To.Add("galenec444@ketchet.com");

                    try
                    {
                        smtpClient.Send(message);
                        return "Success";
                    }
                    catch (ApplicationException ex)
                    {
                        return "Faild";
                        //Error, could not send the message
                    }
                }

            }
        }


        public async Task SendmailTest(string From, dynamic To, string subject, string plainTextContent, string htmlContent)
        {



            // var apiKey = "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";

            //var apiKey = ConfigurationManager.AppSettings["sendgridkey"];
            //string apiKey2 = System.Configuration.ConfigurationManager.AppSettings.Get("sendgridkey");
           // var apiKey2 = new NetworkCredential(_mailSetting.ApiKey);
            var apiKey2 = _mailSetting.ApiKey;
            //var apiKey2 =   System.Configuration.ConfigurationManager.AppSettings.Get("sendgridkey");
            var client = new SendGridClient(apiKey2);
           //var from = new EmailAddress(From); 
            var to = new EmailAddress(To);
            htmlContent =   htmlContent ;
           // var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

             List<EmailAddress> listemail = new List<EmailAddress>();
             listemail.Add(to);


            //send mail by sendgrid

             SendGridMessage msg = new SendGridMessage();
            //msg.SetFrom(new EmailAddress( "sharmamahesh2324@gmail.com"));
            msg.SetFrom(new EmailAddress(From));

             msg.AddTos(listemail);
           // msg.HtmlContent



            //if (CCs.Count > 0)
            //{
            //    msg.AddCcs(CCs);
            //}
         
             msg.SetSubject(subject);
            msg.AddContent(MimeType.Html , htmlContent);
           

            try
            {
                var response = await client.SendEmailAsync(msg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }

}
