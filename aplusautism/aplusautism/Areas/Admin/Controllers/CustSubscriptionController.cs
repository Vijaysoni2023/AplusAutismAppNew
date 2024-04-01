using aplusautism.Service;
using aplusautism.Bal.DTO;
using aplusautism.Bal.ILogic;
using aplusautism.Data;
using aplusautism.Data.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;
using aplusautism.Common.Enums;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Nancy;
using System.Net.Http;
using System.Net;
using aplusautism.Bal.Logic;
using Microsoft.Extensions.Options;
using aplusautism.Setting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Web.Administration;
using Windows.Security.ExchangeActiveSyncProvisioning;

namespace aplusautism.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustSubscriptionController : Controller
    {

        // for getting file Path
        private IHostingEnvironment _hostingEnvironment;

        private readonly IAzureStorage _storage;
        protected Iclientlogic _Iclientlogic { get; private set; }

        public IlessondetailsBycategory _IlessondetailsBycategory { get; private set; }
        public IWebHostEnvironment Environment { get; private set; }
        protected Iuserlogic _IuserLogicBAL { get; private set; }


        protected ISubscriptionsetup _ISubscriptionsetup { get; private set; }
        protected IGlobalCodeCategorylogic _IGlobalCodeCategory { get; private set; }
        public Iadmindashboard _Iadmindashboard { get; private set; }
        private readonly IMailService _mailService;
        private readonly MailSetting _mailSetting;
        private readonly AppSettingsDTO _appSettings;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public AplusautismDbContext _dbcontext;

     


        public CustSubscriptionController(Iuserlogic iuserLogicBAL, IOptions<MailSetting> mailSetting, Iadmindashboard iadmindashboard, IOptions<AppSettingsDTO> appSetting,
            IGlobalCodeCategorylogic IGlobalCodeCategory, IAzureStorage storage,
            IMailService mailService, IHostingEnvironment hostingEnvironment, 
            IWebHostEnvironment _environment, ISubscriptionsetup iSubscriptionsetup,
            IHttpContextAccessor httpContextAccessor, Iclientlogic iclientlogic, AplusautismDbContext dbcontext, IlessondetailsBycategory IlessondetailsBycategory
            )
        {
            _storage = storage;
            _appSettings = appSetting.Value;
            _mailSetting = mailSetting.Value;
            _Iadmindashboard = iadmindashboard;
            _IuserLogicBAL = iuserLogicBAL;
            _IGlobalCodeCategory = IGlobalCodeCategory;
            _mailService = mailService;
            Environment = _environment;
            _hostingEnvironment = hostingEnvironment;
            _ISubscriptionsetup = iSubscriptionsetup;
            _httpContextAccessor = httpContextAccessor;
            _Iclientlogic = iclientlogic;
            _dbcontext = dbcontext;
            _IlessondetailsBycategory = IlessondetailsBycategory;
        }
        public IActionResult Index()
        {
            // RegisterUserDTO usr = new RegisterUserDTO();
            return View();
        }


        public IActionResult LogOutUser()
        {
            return RedirectToAction("SignIn", "Auth", new { area = "", @Registereduser = "" });
        }
        //[HttpPost]
        public IActionResult Plans()
        {

          var subsriptiondata=  _ISubscriptionsetup.GetSubscriptionData();
          return View(subsriptiondata);
        }


        [HttpPost]
        public IActionResult EditPlans(int subscriptionId )
        {

            var subsriptiondata = _ISubscriptionsetup.EditSubscriptionData(subscriptionId);
            return Ok(subsriptiondata);
        }


        [HttpPost]
        public IActionResult SaveDeviceTracking(string DeviceId)
        {
            string DeviceIds = "";
            string DeviceIdName = "";

            if (!string.IsNullOrEmpty(DeviceId))
            {
                string[] DeviceList = DeviceId.Split(",");
                DeviceIds = DeviceList[0];
                DeviceIdName = DeviceList[1];
            }

            string ABUserID = HttpContext.Session.GetString("AB_UserID");
            DeviceTrackingDTO deviceTrackingDTO = new DeviceTrackingDTO();

            deviceTrackingDTO.DeviceId = DeviceIds.ToString();
            deviceTrackingDTO.DeviceName = DeviceIdName.ToString();
            deviceTrackingDTO.UserName = ABUserID.ToString();
            deviceTrackingDTO.UserId = ABUserID.ToString();
            deviceTrackingDTO.CreatedBy = 1;
            var status = _ISubscriptionsetup.SaveDeviceTracking(deviceTrackingDTO);
            return Ok("Ok");
        }

        [HttpPost]
        public IActionResult UpdatePlan(UpdateSubscriptionPlanDTO dataToPost)
        {
            

            var subsriptiondata = _ISubscriptionsetup.UpdateSubscriptionData(dataToPost);
            return Ok(subsriptiondata);
        }



        public IActionResult saveSubscription()
        {
            SubscriptionSetup SubsList = new SubscriptionSetup
            {
                ActivationStart = DateTime.Now,
                ActivationEnd = DateTime.Now,
                SchemeName = "Gold",
                CreatedDate = DateTime.Now,
                Duration = 60,

            };

            _IuserLogicBAL.Insertsubsription(SubsList);
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterUser(RegisterUserDTO registerUserDTO)
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            var tomail = registerUserDTO.Email;

            var frommail = _mailSetting.SenderEmail;
            ContactLogDTO contactLogDTO = new ContactLogDTO();
            var isexituser = _IuserLogicBAL.GetLoginUserDetails(registerUserDTO.Email,"2");

            if (isexituser.count <= 0)
            {
                var response = _IuserLogicBAL.RegisterUser(registerUserDTO);
                if (response == "Success")
                {
                    // return Redirect("https://localhost:7253/");

                    // Send Notifications for Register

                    string startupPath = System.IO.Directory.GetCurrentDirectory();
                    startupPath = startupPath + "\\wwwroot\\EmailTemplates\\";
                    string emailFamilTemplateCheck = System.IO.File.ReadAllText(Path.Combine(startupPath, "RegisterSuccessfullytemplate.html"));
                    //var emailstatus = _mailService.SendRegisterEmail(registerUserDTO.Email, emailFamilTemplateCheck);
                    var emailstatus = _mailService.SendmailTest(frommail, tomail, "Register Notification", "", emailFamilTemplateCheck);

                    //string EmailBody = "Dear Client, <br> Your Trial time period is over. To continue with our lessons please subscribe to our service.<br> Thanks,<br>The A+ Autism team.<br> aplusautism@gmail.com <br>https://www.aplusautism.com/<br>";
                    //_mailService.SendRegisterEmail(registerUserDTO.Email, EmailBody);

                    return RedirectToAction("SignIn", "Auth", new { area = "", @Registereduser = "yes" });
                }
            }
            //ViewBag.UserExsit = true;
            // return Redirect("https://localhost:7253/Auth/Register");
            return RedirectToAction("Register", "Auth",new { area = "", @isexitparameter = "yes" });
        }
        public IActionResult SignIn(LoginDTO login)
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;

                GetIp();

                if (login.Email != null && login.Password != null)
                {
                  
                       
                    var checkes = _IuserLogicBAL.CheckForUser(login.Email, login.Password);


                    var Langauge = _IuserLogicBAL.GetUserLangauge(login.Email, login.Password);
                    if (Langauge.Count > 0)
                    {
                        httpContext.Session.SetString("userLangauge", Langauge[0].PreferedLanguage.ToString());

                    }

                    if (checkes.Count == 0)
                    {
                        ViewBag.Error = "Invalid User Name Or Password";
                        return RedirectToAction("SignIn", "Auth", new { area = "", @Registereduser = "error" });
                    }
                    if (checkes.Count > 0 && checkes[0].IsDeleted == false)
                    {


                        if (!string.IsNullOrEmpty(login.IpAddress))
                        {
                            DeviceTrackingDTO deviceTrackingDTO = new DeviceTrackingDTO();
                            deviceTrackingDTO.DeviceId = "";
                            deviceTrackingDTO.DeviceName = "";
                            deviceTrackingDTO.UserName = checkes[0].AB_UserID.ToString();
                            deviceTrackingDTO.UserId = checkes[0].AB_UserID.ToString();
                            deviceTrackingDTO.CreatedBy = 1;
                            deviceTrackingDTO.IpAddress = login.IpAddress.ToString();
                            var status = _ISubscriptionsetup.SaveDeviceTracking(deviceTrackingDTO);

                        }
                        // Device id when user logged in and insert the Device id into table
                        if (!string.IsNullOrEmpty(login.DeviceId))
                        {
                            String[] DeviceIdlist = login.DeviceId.Split(",");

                            string ABUserID = checkes[0].AB_UserID.ToString();
                            DeviceTrackingDTO deviceTrackingDTO = new DeviceTrackingDTO();

                            for (int i = 0; i < DeviceIdlist.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(DeviceIdlist[i].ToString()))
                                {
                                    deviceTrackingDTO.DeviceId = DeviceIdlist[i].ToString();
                                    deviceTrackingDTO.DeviceName = DeviceIdlist[i].ToString();
                                    deviceTrackingDTO.UserName = ABUserID.ToString();
                                    deviceTrackingDTO.UserId = ABUserID.ToString();
                                    deviceTrackingDTO.CreatedBy = 1;
                                    var status = _ISubscriptionsetup.SaveDeviceTracking(deviceTrackingDTO);
                                }
                            }
                        }
                        // END OF INSERT DEVICE ID CODE

                        // For checking Device ID count if user have logged in device more than 3 

                        var deviceidCount = _IuserLogicBAL.GetUserDeviceIdCount(Convert.ToInt32(checkes[0].AB_UserID), "");

                        if (checkes.Count > 0)
                        {
                            //if(checkes[0].Status=="1" || checkes[0].Status == "3" )
                            if (checkes[0].Status.ToString() == "5")
                            {
                                ViewBag.Error = "Your account has been suspended.";
                                return RedirectToAction("SignIn", "Auth", new { area = "", @Registereduser = checkes[0].Status.ToString() });
                            }
                            if (checkes[0].Status == "1" || checkes[0].Status == "3")
                            {

                                if (deviceidCount.Count > 0)
                                {
                                    httpContext.Session.SetString("username", checkes[0].UserName.ToString());

                                    httpContext.Session.SetString("ClientStatus", checkes[0].Status.ToString());

                                    // Setting Values in session for saving in Payment table

                                    httpContext.Session.SetString("AB_MainID", checkes[0].AB_MainID.ToString());
                                    httpContext.Session.SetString("AB_UserID", checkes[0].AB_UserID.ToString());

                                    httpContext.Session.SetString("UserFullName", checkes[0].UserGroup.ToString());

                                    if (checkes[0].UserType == "admin")
                                    {
                                        return RedirectToAction("AdminDashboard");
                                    }
                                    if (checkes[0].UserType == "client")
                                    {
                                        if (deviceidCount[0].PreferedLanguage <= 3)
                                        {


                                            string LessionTitle = "";
                                            var lessionlist = _IlessondetailsBycategory.GetlessonBycategory("1", Langauge[0].PreferedLanguage.ToString(),"","","",0);
                                        foreach (var lession in lessionlist)
                                        {

                                            if (lession.Lessondeatils !=null)
                                            {
                                                for (int i = 0; i < lession.Lessondeatils.Count; i++)
                                                {
                                                    LessionTitle = lession.Lessondeatils[i].LessonTitle.ToString();
                                                }
                                            }
                                        }
                                        // lessionlist[].Lessondeatils.Count
                                        TempData["LeftMenuCheck"] = LessionTitle;
                                            TempData.Keep("LeftMenuCheck");

                                            return RedirectToAction("ClientDashboard", "ClientPortal", new { area = "Client", @isexitparameter = "yes" });
                                        }
                                        else
                                        {
                                            ViewBag.Error = "You have looged in three device already";
                                            return RedirectToAction("SignIn", "Auth", new { area = "", @Registereduser = "extra" });
                                        }
                                    }
                                    //if (deviceidCount[0].PreferedLanguage > 3)
                                    //{
                                    //    ViewBag.Error = "You have looged in three device already";
                                    //    return RedirectToAction("SignIn", "Auth", new { area = "", @Registereduser = "extra" });
                                    //}
                                    //else
                                    //{
                                    //    httpContext.Session.SetString("username", checkes[0].UserName.ToString());

                                    //    httpContext.Session.SetString("ClientStatus", checkes[0].Status.ToString());

                                    //    // Setting Values in session for saving in Payment table

                                    //    httpContext.Session.SetString("AB_MainID", checkes[0].AB_MainID.ToString());
                                    //    httpContext.Session.SetString("AB_UserID", checkes[0].AB_UserID.ToString());

                                    //    httpContext.Session.SetString("UserFullName", checkes[0].UserGroup.ToString());

                                    //    if (checkes[0].UserType == "admin")
                                    //    {
                                    //        return RedirectToAction("AdminDashboard");
                                    //    }
                                    //    if (checkes[0].UserType == "client")
                                    //    {
                                    //        return RedirectToAction("ClientDashboard", "ClientPortal", new { area = "Client", @isexitparameter = "yes" });
                                    //    }
                                    //}
                                }
                            }

                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        ViewBag.Error2 = "Your account has been deleted";
                        return RedirectToAction("SignIn", "Auth", new { area = "", @Registereduser = "error2" });
                    }

                }


                return View();
            //}
            //catch (ApplicationException ex)
            //{
            //    return null;
            //}


        }

        public string GetIp()
        {

            var deviceInformation = new EasClientDeviceInformation();
            string Id = deviceInformation.Id.ToString();


            //var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
            //var ip = _httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            //return GetIp();

            //var _IP = "RemoteIp:" + request.HttpContext.Connection.RemoteIpAddress.ToString() + " - LocalIpAddress:" +
            //      request.HttpContext.Connection.LocalIpAddress;
            //try
            //{
            //    _IP += " IP.AddressFamily:" + Dns.GetHostEntry(Dns.GetHostName()).AddressList[1].ToString();

            //    _IP += " HostName:" + Dns.GetHostEntry(Dns.GetHostName()).HostName;

            //}
            //catch (Exception e)
            //{

            //}

            return Id;
        }


        //private string GetClientIp(HttpRequestMessage request = null)
        //{
        //    request = request ?? Request;

        //    if (request.Properties.ContainsKey("MS_HttpContext"))
        //    {
        //        return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
        //    }
        //    else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
        //    {
        //        RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)this.Request.Properties[RemoteEndpointMessageProperty.Name];
        //        return prop.Address;
        //    }
        //    else if (HttpContext.Current != null)
        //    {
        //        return HttpContext.Current.Request.UserHostAddress;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        public IActionResult SendNotificationAlert(string Userid)
        {
            int intuserid = 0;
            try
            {
                if (Userid != null)
                {
                    intuserid = Convert.ToInt32(Userid);
                }

                var UserDetails = _dbcontext.aB_User.Where(i => i.AB_UserID == intuserid).FirstOrDefault();
               
               
                string UserStatus = UserDetails.Status.ToString();
                string EmailSubject = "Notifcation";
                string EmailBody = "";
                var Email = _dbcontext.aB_User.Where(i => i.AB_UserID == intuserid).FirstOrDefault().UserName;
                string ContactType = "0";
                string ContactMethod = "0";
                string ContactTypeTopic = "0";
                if (UserStatus == "1")
                {
                    //_mailService.SendReminderEmail(UserDetails.UserName, "Dear Client,<br> We’re sending out a reminder about a  payment please choose a valid plan to continue with us.<br>In case of any query reach us in below email:<br> <br> Thanks");
                }
                if (UserStatus == "5")
                {
                    ContactType = Convert.ToInt32(aplusautism.Common.Enums.ContactTypes.Suspend).ToString();
                    ContactMethod = Convert.ToInt32(aplusautism.Common.Enums.ContactTypes.ContactMethod).ToString();
                    ContactTypeTopic = Convert.ToInt32(aplusautism.Common.Enums.ContactTypes.ContactTypeTopic).ToString();
                    EmailBody = "Dear Client, <br> Your Trial time period is over. To continue with our lessons please subscribe to our service.<br> Thanks,<br>The A+ Autism team.<br> aplusautism@gmail.com <br>https://www.aplusautism.com/<br>";
                    _mailService.SendReminderEmail(UserDetails.UserName, EmailBody);
                }
                else if (UserStatus == "3")
                {
                    ContactType = Convert.ToInt32(aplusautism.Common.Enums.ContactTypes.Trial).ToString();
                    ContactMethod = Convert.ToInt32(aplusautism.Common.Enums.ContactTypes.ContactMethod).ToString();
                    ContactTypeTopic = Convert.ToInt32(aplusautism.Common.Enums.ContactTypes.ContactTypeTopic).ToString();
                    EmailBody = "Dear Client,<br> We are giving you a Trail period of seven days. Please do remember to select a Subscription plan that best works for you after this expires.<br> Thanks, <br> The A+ Autism team. <br> aplusautism@gmail.com<br> https://www.aplusautism.com/<br>";
                    _mailService.SendReminderEmail(UserDetails.UserName, EmailBody);
                }
                if (UserStatus == "5" || UserStatus == "3")
                {
                    // For saving in Contact log
                    string ABUserID = HttpContext.Session.GetString("AB_UserID");
                    ContactLogDTO contactLogDTO = new ContactLogDTO();
                    contactLogDTO.AB_MAINID = Convert.ToInt64(Userid);
                    contactLogDTO.Subject = EmailSubject;
                    contactLogDTO.Disposition = EmailBody;
                    contactLogDTO.ContactType = ContactType;
                    contactLogDTO.ContactTypeTopic = ContactTypeTopic;
                    contactLogDTO.ContactMethod = ContactMethod;
                    contactLogDTO.CreatedBy = Convert.ToInt64(ABUserID);
                    contactLogDTO.CreatedDate = DateTime.Now;
                    var status = _IuserLogicBAL.SaveContactLog(contactLogDTO);
                }





                return Ok("Done");
            }
            catch(ApplicationException ex)
            {
                return BadRequest();
            }

            return Ok();
        }


        public IActionResult Suspend(string Userid)
        {
            int intuserid = 0;
            try
            {
                if (Userid != null)
                {
                    intuserid = Convert.ToInt32(Userid);
                }

                var datastaus = _dbcontext.aB_User.Where(i => i.AB_UserID == intuserid).FirstOrDefault();
                datastaus.oldstatus = datastaus.Status;//To Save status before update
                datastaus.Status = "5";
                _dbcontext.SaveChanges();
                string EmailSubject = "Notifcation";
                string EmailBody = " Dear client,<br> <br>Your Trial time period is over. To continue with our lessons please subscribe to our service.<br> Thanks, <br> The A+ Autism team. <br> aplusautism@gmail.com <br> https://www.aplusautism.com/ <br>"; 

                _mailService.SendSuspendEmail(datastaus.UserName, EmailBody);
                string ABUserID = HttpContext.Session.GetString("AB_UserID");
                ContactLogDTO contactLogDTO = new ContactLogDTO();
                contactLogDTO.AB_MAINID = Convert.ToInt64(Userid);
                contactLogDTO.Subject = EmailSubject;
                contactLogDTO.Disposition = EmailBody;
                contactLogDTO.ContactType = Convert.ToInt32(aplusautism.Common.Enums.ContactTypes.Suspend).ToString();//"Suspend Notification"; //ContactTypes.ContactType.ToString();//"ContactUs";
                contactLogDTO.ContactTypeTopic = Convert.ToInt32(aplusautism.Common.Enums.ContactTypes.ContactTypeTopic).ToString();//"Suspend Notification";
                contactLogDTO.ContactMethod = Convert.ToInt32(aplusautism.Common.Enums.ContactTypes.ContactMethod).ToString();//"Suspend Notification";
                contactLogDTO.CreatedBy = Convert.ToInt64(ABUserID);
                contactLogDTO.CreatedDate = DateTime.Now;
                var status = _IuserLogicBAL.SaveContactLog(contactLogDTO);


                return Ok("Done");
            }
            catch (ApplicationException ex)
            {
                return BadRequest();
            }

            return Ok();
        }
        //public IActionResult Deleted(string Userid)
        //{
        //    int intuserid = 0;
        //    try
        //    {
        //        if (Userid != null)
        //        {
        //            intuserid = Convert.ToInt32(Userid);
        //        }

        //        var datastaus = _dbcontext.aB_User.Where(i => i.AB_UserID == intuserid).FirstOrDefault();
        //        //datastaus.oldstatus = datastaus.Status;//To Save status before update
        //        //datastaus.Status = "5";
        //        datastaus.IsDeleted = true;
        //        _dbcontext.SaveChanges();
        //        string EmailSubject = "Notifcation";
        //        string EmailBody = " Dear client,<br> <br>Your Trial time period is over. To continue with our lessons please subscribe to our service.<br> Thanks, <br> The A+ Autism team. <br> aplusautism@gmail.com <br> https://www.aplusautism.com/ <br>";

        //        _mailService.SendSuspendEmail(datastaus.UserName, EmailBody);
        //        string ABUserID = HttpContext.Session.GetString("AB_UserID");
        //        ContactLogDTO contactLogDTO = new ContactLogDTO();
        //        contactLogDTO.AB_MAINID = Convert.ToInt64(Userid);
        //        contactLogDTO.Subject = EmailSubject;
        //        contactLogDTO.Disposition = EmailBody;
        //        contactLogDTO.ContactType = Convert.ToInt32(aplusautism.Common.Enums.ContactTypes.Suspend).ToString();//"Suspend Notification"; //ContactTypes.ContactType.ToString();//"ContactUs";
        //        contactLogDTO.ContactTypeTopic = Convert.ToInt32(aplusautism.Common.Enums.ContactTypes.ContactTypeTopic).ToString();//"Suspend Notification";
        //        contactLogDTO.ContactMethod = Convert.ToInt32(aplusautism.Common.Enums.ContactTypes.ContactMethod).ToString();//"Suspend Notification";
        //        contactLogDTO.CreatedBy = Convert.ToInt64(ABUserID);
        //        contactLogDTO.CreatedDate = DateTime.Now;
        //        var status = _IuserLogicBAL.SaveContactLog(contactLogDTO);


        //        return Ok("Done");
        //    }
        //    catch (ApplicationException ex)
        //    {
        //        return BadRequest();
        //    }

        //    return Ok();
        //}
        public IActionResult ActiveClientStatus(string Userid)
        {
            int intuserid = 0;
            try
            {
                if (Userid != null)
                {
                    intuserid = Convert.ToInt32(Userid);
                }

                var datastaus = _dbcontext.aB_User.Where(i => i.AB_UserID == intuserid).FirstOrDefault();
                string oldStatus = datastaus.oldstatus;
                // if old status is active
                if (oldStatus == "1")
                {
                    datastaus.Status = "1";// "1"; Status toggled back to Customer
                }
                else if (oldStatus == "3")
                {
                    datastaus.Status = "3";// "1"; Status toggled back to Customer
                }
                //datastaus.Status = datastaus.oldstatus == null?"1": datastaus.oldstatus;// "1"; Status toggled back to Customer
                  datastaus.IsDeleted = false;
                _dbcontext.SaveChanges();

                var abmainDelete= _dbcontext.Ab_main.Where(i => i.AB_MainID == datastaus.AB_MainID).FirstOrDefault();
                abmainDelete.IsDeleted = false;
                _dbcontext.SaveChanges();
                // datastaus.UserName
                string EmailSubject = "Notifcation";
                string EmailBody = " Dear client,<br> <br>Your account has been Activated. In case of any query please reach us in below email:  <br> Thanks, <br> The A+ Autism team. <br> aplusautism@gmail.com <br> https://www.aplusautism.com/ <br> ";
                _mailService.SendSuspendEmail(datastaus.UserName, EmailBody);
                

                // for save email detail in contect log table 

                // getting logged in id
                string ABUserID = HttpContext.Session.GetString("AB_UserID");
                ContactLogDTO contactLogDTO = new ContactLogDTO();
                contactLogDTO.AB_MAINID = Convert.ToInt64(Userid);
                contactLogDTO.Subject = EmailSubject;
                contactLogDTO.Disposition = EmailBody;
                contactLogDTO.ContactType = Convert.ToInt32(aplusautism.Common.Enums.ContactTypes.Active).ToString();//"Active Notification"; //ContactTypes.ContactType.ToString();//"ContactUs";
                contactLogDTO.ContactTypeTopic = Convert.ToInt32(aplusautism.Common.Enums.ContactTypes.ContactTypeTopic).ToString();//"Active Notification";
                contactLogDTO.ContactMethod = Convert.ToInt32(aplusautism.Common.Enums.ContactTypes.ContactMethod).ToString();//"Active Notification";
                contactLogDTO.CreatedBy = Convert.ToInt64(ABUserID);
                contactLogDTO.CreatedDate= DateTime.Now;
                var status = _IuserLogicBAL.SaveContactLog(contactLogDTO);

                return Ok("Done");
            }
            catch (ApplicationException ex)
            {
                return BadRequest();
            }

            return Ok();
        }


        public IActionResult ExtendTimeperiod(string Userid)
        {
            int intuserid = 0;
            try
            {
                if (Userid != null)
                {
                    intuserid = Convert.ToInt32(Userid);
                }

                var datastaus = _dbcontext.aB_User.Where(i => i.AB_UserID == intuserid).FirstOrDefault();

                datastaus.CreatedDate = DateTime.Now.AddDays(7); ;
                _dbcontext.SaveChanges();

                return Ok("Done");
            }
            catch (ApplicationException ex)
            {
                return BadRequest();
            }

            return Ok();
        }


        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult EmailForgetMailSend(string Username)
        {
            var data = _IuserLogicBAL.ForgetPassword(Username);
            string MailMessage = "";
           
            if (data != null && data.Count()>0)
            {
                var result = _mailService.SendEmailAsync(Username, data.FirstOrDefault().Password, data.FirstOrDefault().FirstName, MailMessage);
                if (result == "Success")
                {
                    return Json(true);
                }
                else {
                    return Json(false);
                }
            }
            else
            {
                return Json(false);
            }

            return RedirectToAction("ForgetPassword");
        }
        public IActionResult LessonDetail()
        {
            // var Globalresponse = _IGlobalCodeCategory.GetAll();
            var LessonResponse = _IGlobalCodeCategory.GetLessonSetupDetail();

            var Languagess = _IGlobalCodeCategory.GetGlobalCodesByGlobalCodeCategoryValue("8", "2", "", "", "");
            var gcTitle = _IGlobalCodeCategory.GetGlobalCodesByGlobalCodeCategoryValue("9", "2", "", "", "");

            LessonViewDTO _LessonViewDTO = new LessonViewDTO();
            _LessonViewDTO.Language = Languagess;
            _LessonViewDTO.LessonTitle = gcTitle;
            _LessonViewDTO.Lessonsetup = LessonResponse.OrderByDescending(i=>i.LessonSetupID).ToList();

            return View(_LessonViewDTO);
        }
        [HttpPost]
        [DisableRequestSizeLimit]
        public IActionResult PostLesson( VedioUpload lessonViewDTO, TrailVideoUpload trailVideoUploadDTO, MobileVideoUpload PostMobileLessonDetailDTO, string button, IFormFile img_Upload)
        {

            

            if (lessonViewDTO.LessonSetupId != null && lessonViewDTO.LessonSetupId >0)
            {
                var Updated = _IGlobalCodeCategory.UpdateLessonDetail(lessonViewDTO, trailVideoUploadDTO, PostMobileLessonDetailDTO, button);
           
                if (Updated == "Updated")
                {
                    return RedirectToAction("LessonDetail");
                }
                else
                {
                    ViewBag.NotUpdated = "Faild To Update";
                    return RedirectToAction("LessonDetail");
                }
            }
            //string path = fc["Video"];

            if (lessonViewDTO == null)
            {
                return RedirectToAction("SignIn", "Auth", new { area = "", @Registereduser = "" });
            }
            var response = _IGlobalCodeCategory.InsertLesson(lessonViewDTO, trailVideoUploadDTO, PostMobileLessonDetailDTO, button);
            if (response == "Success")
            {
                return RedirectToAction("LessonDetail");
            }
            else
            {
                return RedirectToAction("SignIn", "Auth", new { area = "", @Registereduser = "" });
            }
           // return null;
        }


        [HttpPost]
       // [RequestFormLimits(MultipartBodyLengthLimit = 104857600)]
        public ActionResult postfile(List<IFormFile> file)
        {
            var filelist = HttpContext.Request.Form.Files;
            //try
            //{
            //    if (await _bufferedFileUploadService.UploadFile(file))
            //    {
            //        ViewBag.Message = "File Upload Successful";
            //    }
            //    else
            //    {
            //        ViewBag.Message = "File Upload Failed";
            //    }
            //}
            //catch (Exception ex)
            //{
            //    //Log ex
            //    ViewBag.Message = "File Upload Failed";
            //}
            return null;
        }


        public string DeleteLesson(int LessonSetupID)
        {
            var IsDeleted = _IGlobalCodeCategory.RemoveLesson("", "4", "", LessonSetupID, "");
            if (IsDeleted == "Success")
            {
                return "Success";
            }
            return "failed";
        }

        public string FavoriteLesson(int LessonSetupID,string LessonLanguage)
        {
            var Isfav = _IGlobalCodeCategory.RemoveLesson("", "10", LessonLanguage, LessonSetupID, "");
            if (Isfav == "Success")
            {
                return "Success";
            }
            return "failed";
        }
        public string NotFavoriteLesson(int LessonSetupID, string LessonLanguage)
        {
            var Isfav = _IGlobalCodeCategory.RemoveLesson("", "11", LessonLanguage, LessonSetupID, "");
            if (Isfav == "Success")
            {
                return "Success";
            }
            return "failed";
        }



        public string DeletClient(int clientid)
        {
            var clientdata = _Iclientlogic.DeleteClient(clientid);
            if (clientdata == "Success")
            {
                return "Success";
            }
            return "failed";
        }



        public PostLessonDetailDTO EditLesson(int LessonSetUpId)
        {
           
            var respons = _IGlobalCodeCategory.GetLessonEditDetail("1", 6, "", LessonSetUpId, "");
            PostLessonDetailDTO BindMOdelData = new PostLessonDetailDTO
            {
               LanguageId = respons.FirstOrDefault().LessonLanguage,
               Description = respons.FirstOrDefault().LessonDescription,
               Title = respons.FirstOrDefault().LessonTitle,
               LessonSetupId = respons.FirstOrDefault().LessonSetupID,
               LessonCategoryId= respons.FirstOrDefault().LessonCategoryId,
               videopath= _appSettings.Azure_Path+ respons.FirstOrDefault().LessonVideoPath,
               SortOrder = respons.FirstOrDefault().SortOrder
                //videodata = _appSettings.Azure_Path + respons.FirstOrDefault().LessonVideoPath,

            };
            return BindMOdelData;
        }
        public LessonViewDTO SearchingStatus(string SearchLetter)
        { 
          var data = _IGlobalCodeCategory.GetSearchLetter("", "5", SearchLetter, 0, "");
            LessonViewDTO _LessonViewDTO = new LessonViewDTO();
            if (data != null)
            {
                _LessonViewDTO.Lessonsetup = data;
                return _LessonViewDTO;
            }
            return _LessonViewDTO;
        }
        //[HttpGet]
        //public ActionResult UploadVideo()
        //{
        //    List<PostLessonDetailDTO> videolist = new List<PostLessonDetailDTO>();
        //    videolist.FirstOrDefault().Video
        //    string CS = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        //    using (SqlConnection con = new SqlConnection(CS))
        //    {
        //        SqlCommand cmd = new SqlCommand("spGetAllVideoFile", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        con.Open();
        //        SqlDataReader rdr = cmd.ExecuteReader();
        //        while (rdr.Read())
        //        {
        //            VideoFiles video = new VideoFiles();
        //            video.ID = Convert.ToInt32(rdr["ID"]);
        //            video.Name = rdr["Name"].ToString();
        //            video.FileSize = Convert.ToInt32(rdr["FileSize"]);
        //            video.FilePath = rdr["FilePath"].ToString();

        //            videolist.Add(video);
        //        }
        //    }
        //    return View(videolist);
        //}

        
        public IActionResult LiveSession( )
        {
            //string response = null;
            return View();

            //string message = "SUCCESS";
            //{
            //    return ("hello world this is live session");
            //}
            
        }


        public IActionResult ClientDeatils(string clientstatus )
        {
           
            string Opcode = "1";
            if(clientstatus!=null)
            {
                Opcode = "2";
            }
            if(clientstatus==null)
            {
                clientstatus = "All";
            }

            // getting list of Status for Clients.

            var statusList = _IGlobalCodeCategory.GetClientStatusList("1");
 
            ViewBag.ClientStatusList = statusList;
            ViewBag.Status = clientstatus;
           
            var clientdata = _Iclientlogic.GetClientList(Opcode, clientstatus);

            //if (clientdata.Count > 0)
            //{
            //    ViewBag.clientdatalist = clientdata;
            //}

            return View(clientdata);
        }


        public IActionResult Activeclients(string status)
        {

            
            ViewBag.Status = status;
            
            var clientdata = _Iclientlogic.GetClientList("2", status);
            return View(clientdata);
        }

    
        public IActionResult ClientProfile( string ClientId,string Clientstatus)
        {
            // for getting Client Details
            ViewBag.Clientstatus = Clientstatus;
            var clientdata = _Iclientlogic.GetClientList("3", ClientId);

            var subsriptiondata = _ISubscriptionsetup.GetSubscriptionData();
            var LastestSubscriptionlist = _ISubscriptionsetup.GetLatestSubscriptionData(ClientId);

            if (LastestSubscriptionlist.Count > 0)
            {
                ViewBag.LastestSubscriptionlist = LastestSubscriptionlist;            
            }
            if(Clientstatus == "'Payment'")
            {
                ViewBag.backbutton = "Payment";
            }
            else
            {
                ViewBag.backbutton = "Client";
            }
            // return View(subsriptiondata);
            return View(clientdata.FirstOrDefault());
            
        }

    
        public IActionResult ChangePassword(LoginDTO login)

        {
            if (login != null)
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                login.Email = httpContext.Session.GetString("username");
                if (login.Email != null && login.Password != null)
                {
                    var Islooged = _IuserLogicBAL.CheckForUser(login.Email, login.Password);

                    if (Islooged.Count > 0)
                    {
                        var success = _IuserLogicBAL.ChangePwd(login);
                        ViewBag.Success = "true";
                        return View();
                    }
                    else
                    {
                        ViewBag.Success = "false";
                        return View();
                    }

                }

            }
            return View();

        }

        public IActionResult AdminDashboard()
        {
            var ClientdashboardStatusCount = _Iadmindashboard.getclientstatusfordashboard("8");

         

            var ClientdashboardPayments = _Iadmindashboard.getclientstatusfordashboard("9");

            AdmindashboardViewmodelDTO objdashbaord = new AdmindashboardViewmodelDTO();

            objdashbaord.ClientStatusList = ClientdashboardStatusCount;
            objdashbaord.DashBoardpayment = ClientdashboardPayments;
            return View(objdashbaord);
        }

        public IActionResult PaymentDetails(PaymentsDTO paymentsDTO)
        {
            string PaymentD = paymentsDTO.StartDate.ToString("dd/MMM/yyyy");
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;
            List<PaymentsDTO>  PaymentDetailslist = new List<PaymentsDTO>();
            if (PaymentD == "01/Jan/0001")
            {
                 PaymentDetailslist = _IuserLogicBAL.GetPaymentsDetails("1", StartDate,EndDate);
            }
            else
            {
                StartDate = paymentsDTO.StartDate;
                EndDate = paymentsDTO.EndDate;
                PaymentDetailslist = _IuserLogicBAL.GetPaymentsDetails("2", StartDate, EndDate);
            }
            if (PaymentDetailslist.Count > 0)
            {
                ViewBag.PaymentDetailslist = PaymentDetailslist;
            }

            return View();
        }
    }


    public class errorcheck
    {
        public string Error { get; set; }
    }
}

