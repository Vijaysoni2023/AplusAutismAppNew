
using aplusautism.Areas.Client.Payment;
using aplusautism.Bal.DTO;
using aplusautism.Bal.ILogic;
using aplusautism.Common.Enums;
using aplusautism.Data;
using aplusautism.Data.Models;
using aplusautism.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using aplusautism.Common.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;
using NuGet.Protocol.Plugins;
using static aplusautism.Service.MailService;
using aplusautism.Setting;
using Microsoft.Extensions.Options;
using aplusautism.Models;
using static aplusautism.Models.DRM_Response;

namespace aplusautism.Areas.Client.Controllers
{

    [Area("Client")]
    public class ClientPortalController : Controller
    {

        public IlessondetailsBycategory _IlessondetailsBycategory { get; private set; }
        protected ISubscriptionsetup _ISubscriptionsetup { get; private set; }

        private IHttpContextAccessor _httpContextAccessor;
        private readonly AplusautismDbContext _DbContext;
        protected Iuserlogic _IuserLogicBAL { get; private set; }


        protected Iclientlogic _IclientlogicBAL { get; private set; }

        private readonly IMailService _mailService;
        private readonly MailSetting _mailSetting;
        private readonly AppSettingsDTO _appSettings;
        //private readonly IMailSetting _mailSetting;

        protected IGlobalCodeCategorylogic _IGlobalCodeCategory { get; private set; }
        public ClientPortalController(IlessondetailsBycategory IlessondetailsBycategory, IOptions<MailSetting> mailSetting, IOptions<AppSettingsDTO> appSetting, ISubscriptionsetup ISubscriptionsetup,
            IHttpContextAccessor httpContextAccessor, AplusautismDbContext dbContext, Iuserlogic iuserLogicBAL,
            IMailService mailService, IGlobalCodeCategorylogic iGlobalCodeCategory, Iclientlogic iclientlogicBAL)
        {
            _appSettings = appSetting.Value;
            _mailSetting = mailSetting.Value;
            _IlessondetailsBycategory = IlessondetailsBycategory;
            // _mailSetting = mailSetting;
            _ISubscriptionsetup = ISubscriptionsetup;
            _httpContextAccessor = httpContextAccessor;
            _DbContext = dbContext;
            _IuserLogicBAL = iuserLogicBAL;
            _mailService = mailService;
            _IGlobalCodeCategory = iGlobalCodeCategory;
            _IclientlogicBAL = iclientlogicBAL;

        }
        public IActionResult ClientDetails()
        {
            return View();
        }

        public IActionResult Contactus()
        {

            string userId = HttpContext.Session.GetString("AB_UserID");

            string userMainId = HttpContext.Session.GetString("AB_MainID");
            string userLangaugeId = HttpContext.Session.GetString("userLangauge");
            List<LessonDetailsbycategoryDTO> lessionlist = new List<LessonDetailsbycategoryDTO>();
            var UserStatus = _IlessondetailsBycategory.GetUserPaymentStatus("1", userId, userMainId);

            if (UserStatus.Count > 0)
            {

                lessionlist = _IlessondetailsBycategory.GetlessonBycategory("1", userLangaugeId, "No", userId, userMainId, Convert.ToInt32(UserStatus.Count));

            }
            else
            {
                lessionlist = _IlessondetailsBycategory.GetlessonBycategory("1", userLangaugeId, "No", userId, userMainId, Convert.ToInt32(UserStatus.Count));
            }

            // var lessionlist = _IlessondetailsBycategory.GetlessonBycategory("1", userLangaugeId, "", "", "", 0);
            List<string> lessionname = new List<string>();
            string LessionNames = "";
            if (!string.IsNullOrEmpty(lessionlist[0].LessonNamesValue.ToString()))
            {
                string[] valuelist = lessionlist[0].LessonNamesValue.Split(", ");
                foreach (string value in valuelist)
                {
                    // lessionname = value
                    lessionname.Add(value);
                    LessionNames += value + ",";

                }

                ViewBag.Results = lessionname;
                TempData["LeftMenuCheck"] = lessionname;

                TempData.Keep("LeftMenuCheck");
            }

            var statusList = _IGlobalCodeCategory.GetClientStatusList("2");

            if (statusList.Count > 0)
            {
                ViewBag.GenericEmail = statusList[0].GlobalCodeName;
            }

            return View();
        }

        [HttpPost]
        public IActionResult Contactus(ContactUsDTO contactUsDTO)
        {
            //HttpContext httpContext = _httpContextAccessor.HttpContext;
            //var frommail = httpContext.Session.GetString("username");
            //var tomail = _mailSetting.SenderEmail;

            //_mailService.SendmailTest(frommail, tomail, contactUsDTO.Subject ,
            //   "",contactUsDTO.Description);
            //// for INsernt and send Emails.
            //ContactLogDTO contactLogDTO  = new ContactLogDTO();


            try
            {
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                var FromUser = httpContext.Session.GetString("username");
                var tomail = _mailSetting.SenderEmail;
                var frommail = _mailSetting.SenderEmail;

                //_mailService.SendmailTest(frommail, tomail, contactUsDTO.Subject,
                //  "", contactUsDTO.Description);
                // for INsernt and send Emails.
                ContactLogDTO contactLogDTO = new ContactLogDTO();

                if (contactUsDTO != null)
                {

                    contactUsDTO.Description += "From " + FromUser + " " + contactUsDTO.Description;
                    //var mailstatus = _mailService.SendContactUsEmail(contactUsDTO.Email, contactUsDTO.Description, contactUsDTO.Subject);
                    _mailService.SendmailTest(frommail, tomail, contactUsDTO.Subject, "", contactUsDTO.Description
                 );
                    // Getting values of User from Session to save in Payment table
                    string Logedinuser = HttpContext.Session.GetString("username");
                    string ABMainID = HttpContext.Session.GetString("AB_MainID");
                    string ABUserID = HttpContext.Session.GetString("AB_UserID");

                    contactLogDTO.AB_MAINID = Convert.ToInt64(ABMainID);
                    contactLogDTO.Subject = contactUsDTO.Subject;
                    contactLogDTO.Disposition = contactUsDTO.Description;
                    contactLogDTO.ContactType = Convert.ToInt32(aplusautism.Common.Enums.ContactTypes.Contactus).ToString();//"ContactUs"; //ContactTypes.ContactType.ToString();//"ContactUs";
                    contactLogDTO.ContactTypeTopic = Convert.ToInt32(aplusautism.Common.Enums.ContactTypes.ContactTypeTopic).ToString();//"ContactUs";
                    contactLogDTO.ContactMethod = Convert.ToInt32(aplusautism.Common.Enums.ContactTypes.ContactMethod).ToString();//"ContactUs";
                    contactLogDTO.CreatedBy = Convert.ToInt64(ABUserID);
                    var status = _IuserLogicBAL.SaveContactLog(contactLogDTO);
                    ViewBag.MailCheck = "Send";

                    var statusList = _IGlobalCodeCategory.GetClientStatusList("2");

                    if (statusList.Count > 0)
                    {
                        ViewBag.GenericEmail = statusList[0].GlobalCodeName;
                    }

                }
                return View();
            }
            catch (ApplicationException ex)
            {
                ViewBag.MailCheck = "NotSend";
                return View();
                //Error, could not send the message
            }
        }


        public async Task<IActionResult> ClientDashboard(string LessonName)

        {
            ViewBag.Player_License = _appSettings.Player_License;
            string userLangaugeId = HttpContext.Session.GetString("userLangauge");

            string userId = HttpContext.Session.GetString("AB_UserID");

            string userMainId = HttpContext.Session.GetString("AB_MainID");

            // var lessionlist = "";
            DRM_Video_Details DRM_Details_Web;
            DRM_Video_Details DRM_Details_Mobile;
            DRM_Video_Details DRM_Details_Trial;
            List<LessonDetailsbycategoryDTO> lessionlist = new List<LessonDetailsbycategoryDTO>();
            var UserStatus = _IlessondetailsBycategory.GetUserPaymentStatus("1", userId, userMainId);

            if (UserStatus.Count > 0)
            {

                // payment done
                #region PaymentDone
                if (string.IsNullOrEmpty(LessonName))
                {
                    lessionlist = _IlessondetailsBycategory.GetlessonBycategory("1", userLangaugeId, "No", userId, userMainId, Convert.ToInt32(UserStatus.Count));
                }
                else
                {
                    lessionlist = _IlessondetailsBycategory.GetlessonBycategory("1", userLangaugeId, LessonName, userId, userMainId, Convert.ToInt32(UserStatus.Count));
                    // lessionlist[0].LessonNamesValue= LessonName;
                }

                // for checking if user is trail or full
                if (lessionlist.Count > 0)
                {
                    if (lessionlist[0].UserStatus == "0")
                    {
                        ViewBag.UserStatus = lessionlist[0].UserStatus.ToString();
                    }
                    else
                    {
                        ViewBag.UserStatus = lessionlist[0].UserStatus.ToString();
                    }
                }


                HttpContext httpContext = _httpContextAccessor.HttpContext;
                string ClientStatus = httpContext.Session.GetString("ClientStatus");
                ViewBag.ClientStatusCheck = ClientStatus;

                string LessionTitle = "";
                string LessonTrailVedio = "";
                string LessonFullVedio = "";
                string LessonMobileVedio = "";

                List<string> lessionname = new List<string>();
                string LessionNames = "";
                if (!string.IsNullOrEmpty(lessionlist[0].LessonNamesValue.ToString()))
                {
                    string[] valuelist = lessionlist[0].LessonNamesValue.Split(", ");
                    foreach (string value in valuelist)
                    {
                       
                            // lessionname = value
                            lessionname.Add(value);
                            LessionNames += value + ",";
                        

                    }

                    ViewBag.Results = lessionname;
                    TempData["LeftMenuCheck"] = lessionname;

                    TempData.Keep("LeftMenuCheck");
                }


                foreach (var lession in lessionlist)
                {
                    if (lession.Lessondeatils != null)
                    {
                        for (int i = 0; i < lession.Lessondeatils.Count; i++)
                        {
                            LessionTitle = lession.Lessondeatils[i].LessonTitle.ToString();

                            // lessionname.Add(LessionTitle);

                            //LessonTrailVedio = _appSettings.Azure_Path + lession.Lessondeatils[i].LessonTrailVideoPath.ToString();
                            LessonTrailVedio = lession.Lessondeatils[i].LessonTrailVideoPath.ToString();
                            //LessonTrailVedio = _appSettings.Azure_Path + "/videos-upload-trialvideo/Final Trial vedio.mp4";
                            //LessonFullVedio = _appSettings.Azure_Path + lession.Lessondeatils[i].LessonVideoPath.ToString();
                            LessonFullVedio = lession.Lessondeatils[i].LessonVideoPath.ToString();
                            // LessonMobileVedio = _appSettings.Azure_Path+lession.Lessondeatils[i].MobileVideoPath.ToString();
                            //ViewBag.Results = lession.Lessondeatils[i].LessonTitle.ToList();

                            // check for Mobile vedio path in case of Trail user and active user 
                            if (ClientStatus == "3")
                            {
                                // LessonMobileVedio = _appSettings.Azure_Path + "/videos-upload-trialvideo/Final Trial vedio.mp4";
                                //LessonMobileVedio = _appSettings.Azure_Path + lession.Lessondeatils[i].LessonTrailVideoPath.ToString();
                                LessonMobileVedio = lession.Lessondeatils[i].LessonTrailVideoPath.ToString();
                            }
                            else if (ClientStatus == "1")
                            {
                                //LessonMobileVedio = _appSettings.Azure_Path + lession.Lessondeatils[i].MobileVideoPath.ToString();
                                LessonMobileVedio = lession.Lessondeatils[i].MobileVideoPath.ToString();
                                //LessonMobileVedio = _appSettings.Azure_Path + lession.Lessondeatils[i].MobileVideoPath.ToString();
                                LessonMobileVedio = lession.Lessondeatils[i].MobileVideoPath.ToString();
                            }
                        }


                        //ViewBag.Results = lessionname;
                    }
                }

                // lessionlist[].Lessondeatils.Count
                ViewBag.LessonTrailVedio = LessonTrailVedio;
                ViewBag.LessonFullVedio = LessonFullVedio;
                ViewBag.LessonMobileVedio = LessonMobileVedio;

                //Get DRMDetails by Cotent Id , which is in Lesson Video Path Column
                DRM_Details_Web = await Get_DRM_DetailsBy_ContentUUID(LessonFullVedio);
                DRM_Details_Mobile = await Get_DRM_DetailsBy_ContentUUID(LessonMobileVedio);
                DRM_Details_Trial = await Get_DRM_DetailsBy_ContentUUID(LessonTrailVedio);

                #endregion

            }
            else
            {
                #region paymentNotDone
                //ADDED ON 21-OCT-2023-START
                if (string.IsNullOrEmpty(LessonName))
                {
                    lessionlist = _IlessondetailsBycategory.GetlessonBycategory("1", userLangaugeId, "No", userId, userMainId, Convert.ToInt32(UserStatus.Count));
                }
                else
                {
                    lessionlist = _IlessondetailsBycategory.GetlessonBycategory("1", userLangaugeId, LessonName, userId, userMainId, Convert.ToInt32(UserStatus.Count));
                    // lessionlist[0].LessonNamesValue= LessonName;
                }
                //ADDED ON 21-OCT-2023-END
                //lessionlist = _IlessondetailsBycategory.GetlessonBycategory("1", userLangaugeId, "No", userId, userMainId, Convert.ToInt32(UserStatus.Count));
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                string ClientStatus = httpContext.Session.GetString("ClientStatus");
                ViewBag.ClientStatusCheck = ClientStatus;

                string LessionTitle = "";
                string LessonTrailVedio = "";
                string LessonFullVedio = "";
                string LessonMobileVedio = "";

                List<string> lessionname = new List<string>();
                string LessionNames = "";
                if (!string.IsNullOrEmpty(lessionlist[0].LessonNamesValue.ToString()))
                {
                    string[] valuelist = lessionlist[0].LessonNamesValue.Split(", ");
                    foreach (string value in valuelist)
                    {
                        // lessionname = value
                        lessionname.Add(value);
                        LessionNames += value + ",";

                    }

                    ViewBag.Results = lessionname;
                    TempData["LeftMenuCheck"] = lessionname;

                    TempData.Keep("LeftMenuCheck");
                }


                foreach (var lession in lessionlist)
                {
                    if (lession.Lessondeatils != null)
                    {
                        for (int i = 0; i < lession.Lessondeatils.Count; i++)
                        {
                            LessionTitle = lession.Lessondeatils[i].LessonTitle.ToString();

                            // lessionname.Add(LessionTitle);

                            LessonTrailVedio = lession.Lessondeatils[i].LessonTrailVideoPath.ToString();
                            //LessonTrailVedio = _appSettings.Azure_Path + "/videos-upload-trialvideo/Final Trial vedio.mp4";
                            LessonFullVedio = lession.Lessondeatils[i].LessonVideoPath.ToString();
                            // LessonMobileVedio = _appSettings.Azure_Path+lession.Lessondeatils[i].MobileVideoPath.ToString();
                            //ViewBag.Results = lession.Lessondeatils[i].LessonTitle.ToList();

                            // check for Mobile vedio path in case of Trail user and active user 
                            if (ClientStatus == "3")
                            {
                                // LessonMobileVedio = _appSettings.Azure_Path + "/videos-upload-trialvideo/Final Trial vedio.mp4";
                                LessonMobileVedio = lession.Lessondeatils[i].LessonTrailVideoPath.ToString();
                            }
                            else if (ClientStatus == "1")
                            {
                                LessonMobileVedio = lession.Lessondeatils[i].MobileVideoPath.ToString();
                            }
                        }


                        //ViewBag.Results = lessionname;
                    }
                }

                // lessionlist[].Lessondeatils.Count
                ViewBag.LessonTrailVedio = LessonTrailVedio;
                ViewBag.LessonFullVedio = LessonFullVedio;
                ViewBag.LessonMobileVedio = LessonMobileVedio;

                DRM_Details_Web = await Get_DRM_DetailsBy_ContentUUID(LessonFullVedio);
                DRM_Details_Mobile = await Get_DRM_DetailsBy_ContentUUID(LessonMobileVedio);
                DRM_Details_Trial = await Get_DRM_DetailsBy_ContentUUID(LessonTrailVedio);


                #endregion
            }

            ViewBag.DRM_Details_Web = DRM_Details_Web;
            ViewBag.DRM_Details_Mobile = DRM_Details_Mobile;
            ViewBag.DRM_Details_Trial = DRM_Details_Trial;
            return View(lessionlist);
        }
        public async Task<IActionResult> ClientDashboard9(string LessonName)

        {
            ViewBag.Player_License = _appSettings.Player_License;
            string userLangaugeId = HttpContext.Session.GetString("userLangauge");

            string userId = HttpContext.Session.GetString("AB_UserID");

            string userMainId = HttpContext.Session.GetString("AB_MainID");

            // var lessionlist = "";
            DRM_Video_Details DRM_Details_Web;
            DRM_Video_Details DRM_Details_Mobile;
            DRM_Video_Details DRM_Details_Trial;
            List<LessonDetailsbycategoryDTO> lessionlist = new List<LessonDetailsbycategoryDTO>();
            var UserStatus = _IlessondetailsBycategory.GetUserPaymentStatus("1", userId, userMainId);

            if (UserStatus.Count > 0)
            {

                // payment done
                #region PaymentDone
                if (string.IsNullOrEmpty(LessonName))
                {
                    lessionlist = _IlessondetailsBycategory.GetlessonBycategory("1", userLangaugeId, "No", userId, userMainId, Convert.ToInt32(UserStatus.Count));
                }
                else
                {
                    lessionlist = _IlessondetailsBycategory.GetlessonBycategory("1", userLangaugeId, LessonName, userId, userMainId, Convert.ToInt32(UserStatus.Count));
                    // lessionlist[0].LessonNamesValue= LessonName;
                }

                // for checking if user is trail or full
                if (lessionlist.Count > 0)
                {
                    if (lessionlist[0].UserStatus == "0")
                    {
                        ViewBag.UserStatus = lessionlist[0].UserStatus.ToString();
                    }
                    else
                    {
                        ViewBag.UserStatus = lessionlist[0].UserStatus.ToString();
                    }
                }


                HttpContext httpContext = _httpContextAccessor.HttpContext;
                string ClientStatus = httpContext.Session.GetString("ClientStatus");
                ViewBag.ClientStatusCheck = ClientStatus;

                string LessionTitle = "";
                string LessonTrailVedio = "";
                string LessonFullVedio = "";
                string LessonMobileVedio = "";

                List<string> lessionname = new List<string>();
                string LessionNames = "";
                if (!string.IsNullOrEmpty(lessionlist[0].LessonNamesValue.ToString()))
                {
                    string[] valuelist = lessionlist[0].LessonNamesValue.Split(", ");
                    foreach (string value in valuelist)
                    {
                        // lessionname = value
                        lessionname.Add(value);
                        LessionNames += value + ",";

                    }

                    ViewBag.Results = lessionname;
                    TempData["LeftMenuCheck"] = lessionname;

                    TempData.Keep("LeftMenuCheck");
                }


                foreach (var lession in lessionlist)
                {
                    if (lession.Lessondeatils != null)
                    {
                        for (int i = 0; i < lession.Lessondeatils.Count; i++)
                        {
                            LessionTitle = lession.Lessondeatils[i].LessonTitle.ToString();

                            // lessionname.Add(LessionTitle);

                            //LessonTrailVedio = _appSettings.Azure_Path + lession.Lessondeatils[i].LessonTrailVideoPath.ToString();
                            LessonTrailVedio = lession.Lessondeatils[i].LessonTrailVideoPath.ToString();
                            //LessonTrailVedio = _appSettings.Azure_Path + "/videos-upload-trialvideo/Final Trial vedio.mp4";
                            //LessonFullVedio = _appSettings.Azure_Path + lession.Lessondeatils[i].LessonVideoPath.ToString();
                            LessonFullVedio = lession.Lessondeatils[i].LessonVideoPath.ToString();
                            // LessonMobileVedio = _appSettings.Azure_Path+lession.Lessondeatils[i].MobileVideoPath.ToString();
                            //ViewBag.Results = lession.Lessondeatils[i].LessonTitle.ToList();

                            // check for Mobile vedio path in case of Trail user and active user 
                            if (ClientStatus == "3")
                            {
                                // LessonMobileVedio = _appSettings.Azure_Path + "/videos-upload-trialvideo/Final Trial vedio.mp4";
                                //LessonMobileVedio = _appSettings.Azure_Path + lession.Lessondeatils[i].LessonTrailVideoPath.ToString();
                                LessonMobileVedio = lession.Lessondeatils[i].LessonTrailVideoPath.ToString();
                            }
                            else if (ClientStatus == "1")
                            {
                                //LessonMobileVedio = _appSettings.Azure_Path + lession.Lessondeatils[i].MobileVideoPath.ToString();
                                LessonMobileVedio = lession.Lessondeatils[i].MobileVideoPath.ToString();
                                //LessonMobileVedio = _appSettings.Azure_Path + lession.Lessondeatils[i].MobileVideoPath.ToString();
                                LessonMobileVedio = lession.Lessondeatils[i].MobileVideoPath.ToString();
                            }
                        }


                        //ViewBag.Results = lessionname;
                    }
                }

                // lessionlist[].Lessondeatils.Count
                ViewBag.LessonTrailVedio = LessonTrailVedio;
                ViewBag.LessonFullVedio = LessonFullVedio;
                ViewBag.LessonMobileVedio = LessonMobileVedio;

                //Get DRMDetails by Cotent Id , which is in Lesson Video Path Column
                DRM_Details_Web = await Get_DRM_DetailsBy_ContentUUID(LessonFullVedio);
                DRM_Details_Mobile = await Get_DRM_DetailsBy_ContentUUID(LessonMobileVedio);
                DRM_Details_Trial = await Get_DRM_DetailsBy_ContentUUID(LessonTrailVedio);

                #endregion

            }
            else
            {
                #region paymentNotDone
                lessionlist = _IlessondetailsBycategory.GetlessonBycategory("1", userLangaugeId, "No", userId, userMainId, Convert.ToInt32(UserStatus.Count));
                HttpContext httpContext = _httpContextAccessor.HttpContext;
                string ClientStatus = httpContext.Session.GetString("ClientStatus");
                ViewBag.ClientStatusCheck = ClientStatus;

                string LessionTitle = "";
                string LessonTrailVedio = "";
                string LessonFullVedio = "";
                string LessonMobileVedio = "";

                List<string> lessionname = new List<string>();
                string LessionNames = "";
                if (!string.IsNullOrEmpty(lessionlist[0].LessonNamesValue.ToString()))
                {
                    string[] valuelist = lessionlist[0].LessonNamesValue.Split(", ");
                    foreach (string value in valuelist)
                    {
                        // lessionname = value
                        lessionname.Add(value);
                        LessionNames += value + ",";

                    }

                    ViewBag.Results = lessionname;
                    TempData["LeftMenuCheck"] = lessionname;

                    TempData.Keep("LeftMenuCheck");
                }


                foreach (var lession in lessionlist)
                {
                    if (lession.Lessondeatils != null)
                    {
                        for (int i = 0; i < lession.Lessondeatils.Count; i++)
                        {
                            LessionTitle = lession.Lessondeatils[i].LessonTitle.ToString();

                            // lessionname.Add(LessionTitle);

                            LessonTrailVedio = lession.Lessondeatils[i].LessonTrailVideoPath.ToString();
                            //LessonTrailVedio = _appSettings.Azure_Path + "/videos-upload-trialvideo/Final Trial vedio.mp4";
                            LessonFullVedio = lession.Lessondeatils[i].LessonVideoPath.ToString();
                            // LessonMobileVedio = _appSettings.Azure_Path+lession.Lessondeatils[i].MobileVideoPath.ToString();
                            //ViewBag.Results = lession.Lessondeatils[i].LessonTitle.ToList();

                            // check for Mobile vedio path in case of Trail user and active user 
                            if (ClientStatus == "3")
                            {
                                // LessonMobileVedio = _appSettings.Azure_Path + "/videos-upload-trialvideo/Final Trial vedio.mp4";
                                LessonMobileVedio = lession.Lessondeatils[i].LessonTrailVideoPath.ToString();
                            }
                            else if (ClientStatus == "1")
                            {
                                LessonMobileVedio = lession.Lessondeatils[i].MobileVideoPath.ToString();
                            }
                        }


                        //ViewBag.Results = lessionname;
                    }
                }

                // lessionlist[].Lessondeatils.Count
                ViewBag.LessonTrailVedio = LessonTrailVedio;
                ViewBag.LessonFullVedio = LessonFullVedio;
                ViewBag.LessonMobileVedio = LessonMobileVedio;

                DRM_Details_Web = await Get_DRM_DetailsBy_ContentUUID(LessonFullVedio);
                DRM_Details_Mobile = await Get_DRM_DetailsBy_ContentUUID(LessonMobileVedio);
                DRM_Details_Trial = await Get_DRM_DetailsBy_ContentUUID(LessonTrailVedio);


                #endregion
            }

            ViewBag.DRM_Details_Web = DRM_Details_Web;
            ViewBag.DRM_Details_Mobile = DRM_Details_Mobile;
            ViewBag.DRM_Details_Trial = DRM_Details_Trial;
            return View(lessionlist);
        }

        public IActionResult LessionDetails()
        {
            string userLangaugeId = HttpContext.Session.GetString("userLangauge");
            var lessionlist = _IlessondetailsBycategory.GetlessonBycategory("1", userLangaugeId, "", "", "", 0);

            return View(lessionlist);
        }


        public IActionResult ChangepassowrdClient(LoginDTO login)

        {

            string userLangaugeId = HttpContext.Session.GetString("userLangauge");

            string userId = HttpContext.Session.GetString("AB_UserID");

            string userMainId = HttpContext.Session.GetString("AB_MainID");

            List<LessonDetailsbycategoryDTO> lessionlist = new List<LessonDetailsbycategoryDTO>();
            var UserStatus = _IlessondetailsBycategory.GetUserPaymentStatus("1", userId, userMainId);

            if (UserStatus.Count > 0)
            {

                lessionlist = _IlessondetailsBycategory.GetlessonBycategory("1", userLangaugeId, "No", userId, userMainId, Convert.ToInt32(UserStatus.Count));

            }
            else
            {
                lessionlist = _IlessondetailsBycategory.GetlessonBycategory("1", userLangaugeId, "No", userId, userMainId, Convert.ToInt32(UserStatus.Count));
            }

            List<string> lessionname = new List<string>();
            string LessionNames = "";
            if (!string.IsNullOrEmpty(lessionlist[0].LessonNamesValue.ToString()))
            {
                string[] valuelist = lessionlist[0].LessonNamesValue.Split(", ");
                foreach (string value in valuelist)
                {
                    // lessionname = value
                    lessionname.Add(value);
                    LessionNames += value + ",";

                }

                ViewBag.Results = lessionname;
                TempData["LeftMenuCheck"] = lessionname;

                TempData.Keep("LeftMenuCheck");
            }

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


        public IActionResult SubscriptionBuy()
        {

            string userLangaugeId = HttpContext.Session.GetString("userLangauge");
            string userId = HttpContext.Session.GetString("AB_UserID");

            string userMainId = HttpContext.Session.GetString("AB_MainID");

            List<LessonDetailsbycategoryDTO> lessionlist = new List<LessonDetailsbycategoryDTO>();
            var UserStatus = _IlessondetailsBycategory.GetUserPaymentStatus("1", userId, userMainId);

            if (UserStatus.Count > 0)
            {

                lessionlist = _IlessondetailsBycategory.GetlessonBycategory("1", userLangaugeId, "No", userId, userMainId, Convert.ToInt32(UserStatus.Count));

            }
            else
            {
                lessionlist = _IlessondetailsBycategory.GetlessonBycategory("1", userLangaugeId, "No", userId, userMainId, Convert.ToInt32(UserStatus.Count));
            }

            List<string> lessionname = new List<string>();
            string LessionNames = "";
            if (!string.IsNullOrEmpty(lessionlist[0].LessonNamesValue.ToString()))
            {
                string[] valuelist = lessionlist[0].LessonNamesValue.Split(", ");
                foreach (string value in valuelist)
                {
                    // lessionname = value
                    lessionname.Add(value);
                    LessionNames += value + ",";

                }

                ViewBag.Results = lessionname;
                TempData["LeftMenuCheck"] = lessionname;

                TempData.Keep("LeftMenuCheck");
            }

            var subsriptiondata = _ISubscriptionsetup.GetSubscriptionData();

            string ABUserID = HttpContext.Session.GetString("AB_UserID");

            var LastestSubscriptionlist = _ISubscriptionsetup.GetLatestSubscriptionData(ABUserID);

            if (LastestSubscriptionlist.Count > 0)
            {
                ViewBag.LastestSubscriptionlist = LastestSubscriptionlist;
                ViewBag.DisableButton = 1;
            }


            return View(subsriptiondata);

        }

        public string SessionLive()
        {
            return "View()";
        }

        public IActionResult LogOutUser()
        {
            return RedirectToAction("SignIn", "Auth", new { area = "", @Registereduser = "" });
        }

        public IActionResult Payment(string SubscriptionSetupID, string Amount)
        {
            string message = "";
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            var tomail = httpContext.Session.GetString("username");

            var frommail = _mailSetting.SenderEmail;
            ContactLogDTO contactLogDTO = new ContactLogDTO();


            string userLangaugeId = HttpContext.Session.GetString("userLangauge");

            var lessionlist = _IlessondetailsBycategory.GetlessonBycategory("1", userLangaugeId, "", "", "", 0);
            string LessionTitle = "";
            foreach (var lession in lessionlist)
            {
                if (lession.Lessondeatils != null)
                {
                    for (int i = 0; i < lession.Lessondeatils.Count; i++)
                    {
                        LessionTitle = lession.Lessondeatils[i].LessonTitle.ToString();
                    }
                }
            }
            // lessionlist[].Lessondeatils.Count
            TempData["LeftMenuCheck"] = LessionTitle;

            ViewBag.Message = message;
            if (!string.IsNullOrEmpty(Amount))
            {
                ViewBag.Amount = Amount.ToString();
            }

            if (!string.IsNullOrEmpty(SubscriptionSetupID))
            {
                ViewBag.SubscriptionSetupID = SubscriptionSetupID.ToString();
            }

            try
            {
                return View();
            }
            catch (ApplicationException ex)     //TODO --- IMPLEMENTATION OF LOGS
            {

                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }


        [Route("SavePayment")]
        [HttpPost]
        public async Task<IActionResult> SavePayment(PaymentsDTO payModel)
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            var tomail = httpContext.Session.GetString("username");

            var frommail = _mailSetting.SenderEmail;
            ContactLogDTO contactLogDTO = new ContactLogDTO();

            try
            {
                // Getting values of User from Session to save in Payment table
                string Logedinuser = HttpContext.Session.GetString("username");
                string ABMainID = HttpContext.Session.GetString("AB_MainID");
                string ABUserID = HttpContext.Session.GetString("AB_UserID");
                var response = "";

                Payments payment = new Payments();

                // Assgin Values for columns
                payment.Email = Logedinuser;
                payment.CustomerName = Logedinuser;
                payment.PaymentDate = DateTime.Now;
                payment.SubscriptionSetupID = payModel.SubscriptionSetupID;

                // for getting Plan Name

                string PaymentDetailDescrption = "";
                string ClientName = "";
                string PlanName = "";
                string ClientEmail = "";

                List<PaymentsDTO> PlanType = new List<PaymentsDTO>();
                PlanType = _IuserLogicBAL.GetPLanName(Convert.ToInt32(payModel.SubscriptionSetupID));

                var clientDetail = _IclientlogicBAL.GetClientDetail(Convert.ToInt32(ABUserID));

                payment.IsActive = true;
                payment.AB_UserID = Convert.ToInt64(ABUserID);
                payment.AB_MainID = Convert.ToInt64(ABMainID);
                payment.Amount = Convert.ToDecimal(payModel.AmountPaid);
                //CREATING CUSTOMER IN DATABASE
                var result = _DbContext.Payments.Add(payment);
                _DbContext.SaveChanges();

                if (PlanType.Count > 0)
                {
                    PlanName = PlanType[0].SchemeName;
                }
                if (clientDetail.Count > 0)
                {
                    ClientName = clientDetail[0].FirstName;
                    ClientEmail = clientDetail[0].StatusName;

                }

                PaymentDetailDescrption = "" + PlanName + " Plan subscribed by " + ClientEmail + " (" + ClientName + ")";
                var Paymentid = payment.Id;

                if (payModel.CardNumder != "")
                {
                    //CREATING CUSTOMER IN STRIPE
                    payModel.CardNumder = payModel.CardNumder.Trim();
                    payModel.Description = PaymentDetailDescrption;
                    response = await PaymentProcess.PayAsync(payModel);

                    // var data = JObject.Parse(response);

                    // IN CASE OF EXCEPTION CATCH PART IS HANDELING IT, TRY PARSE NOT AVAILABLE
                    if (response != null && response != "Your card number is incorrect."
                        && response != "Your card's expiration year is invalid." && response != "The card number is not a valid credit card number.")
                    {

                        var paylist = _IuserLogicBAL.UpdateStripeAPIResponse(Paymentid.ToString(), response.ToString(), true);
                        // for update clinet status when payment is done
                        var clientStatus = _IuserLogicBAL.UpdateClientStatus(ABUserID, ABMainID);

                        string startupPath = System.IO.Directory.GetCurrentDirectory();
                        startupPath = startupPath + "\\wwwroot\\EmailTemplates\\";
                        string emailTemplateCheck = System.IO.File.ReadAllText(Path.Combine(startupPath, "Paymenttemplate.html"));
                        //var emailstatus = _mailService.SendPaymentEmail(Logedinuser.ToString(), emailTemplateCheck);
                        var emailstatus = _mailService.SendmailTest(frommail, tomail, "Payment Notification", "", emailTemplateCheck);

                        return RedirectToAction("Payment", new { message = "Success" });
                    }
                    else
                    {
                        var paylist = _IuserLogicBAL.UpdateStripeAPIResponse(Paymentid.ToString(), response.ToString(), false);
                        string startupPath = System.IO.Directory.GetCurrentDirectory();
                        startupPath = startupPath + "\\wwwroot\\EmailTemplates\\";
                        string emailFamilTemplateCheck = System.IO.File.ReadAllText(Path.Combine(startupPath, "PaymentFailedtemplate.html"));
                        //var emailstatus = _mailService.SendPaymentEmail(Logedinuser.ToString(), emailFamilTemplateCheck);
                        var emailstatus = _mailService.SendmailTest(frommail, tomail, "Payment Notification", "", emailFamilTemplateCheck);

                        return RedirectToAction("Payment", new { message = "Failed" });
                    }

                }
            }
            catch (ApplicationException ex)     //TODO --- IMPLEMENTATION OF LOGS
            {

                ModelState.AddModelError("", ex.Message);
            }
            return RedirectToAction("Payment", new { message = "Failed" });
            // return View();
        }

        //NEW CONTROLLER FOR IOS VIDEO PLAYR BY SURJIT
        public IActionResult ClientDashboard2()
        {
            string userLangaugeId = HttpContext.Session.GetString("userLangauge");
            var lessionlist = _IlessondetailsBycategory.GetlessonBycategory("1", userLangaugeId, "", "", "", 0);

            HttpContext httpContext = _httpContextAccessor.HttpContext;
            string ClientStatus = httpContext.Session.GetString("ClientStatus");
            ViewBag.ClientStatusCheck = ClientStatus;
            string LessionTitle = "";
            string LessonTrailVedio = "";
            string LessonFullVedio = "";
            string LessonMobileVedio = "";
            foreach (var lession in lessionlist)
            {
                for (int i = 0; i < lession.Lessondeatils.Count; i++)
                {
                    LessionTitle = lession.Lessondeatils[i].LessonTitle.ToString();
                    LessonTrailVedio = lession.Lessondeatils[i].LessonTrailVideoPath.ToString();
                    LessonFullVedio = lession.Lessondeatils[i].LessonVideoPath.ToString();
                    LessonMobileVedio = lession.Lessondeatils[i].MobileVideoPath.ToString();
                }
            }
            // lessionlist[].Lessondeatils.Count
            ViewBag.LessonTrailVedio = LessonTrailVedio;
            ViewBag.LessonFullVedio = LessonFullVedio;
            ViewBag.LessonMobileVedio = LessonMobileVedio;

            TempData["LeftMenuCheck"] = LessionTitle;

            return View(lessionlist);

        }
        private async Task<DRM_Video_Details> Get_DRM_DetailsBy_ContentUUID(string Cotent_UUID)
        {
            DRM_Video_Details DRM_Video_Details = new DRM_Video_Details();
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://apigateway.muvi.com/player");
            var Access_Token = await Get_MoviFlex_AuthToken();
            request.Headers.Add("Authorization", "Bearer " + Access_Token);
            var content = new StringContent("{\r\n\r\n\"query\": \"{ mediaDetails(app_token: \\\":app_token\\\", product_key: \\\":product_key\\\", user_uuid: \\\":me\\\", store_key: \\\":store_key\\\", content_uuid: \\\"" + Cotent_UUID + "\\\", language_code: \\\"en\\\", country_code: \\\"US\\\"){ content_asset_type video_details {is_drm mpeg_path hls_path third_party_url duration resume_time subtitle {language_code subtitle_url language_type} }  license {wideVine playReady fairPlay} }}\"\r\n\r\n}", null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            //response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {
                var response_data = response.Content.ReadAsStringAsync().Result;
                var response_test = JsonConvert.DeserializeObject<dynamic>(response_data);
                DRM_Response DRM_Response = JsonConvert.DeserializeObject<DRM_Response>(response_data);
                DRM_Video_Details = new DRM_Video_Details()
                {
                    Video_Source_dash = DRM_Response.data.mediaDetails.video_details.mpeg_path,
                    Video_Source_Hls = DRM_Response.data.mediaDetails.video_details.hls_path,
                    License_fairPlay = DRM_Response.data.mediaDetails.license.fairPlay,
                    License_playReady = DRM_Response.data.mediaDetails.license.playReady,
                    License_wideVine = DRM_Response.data.mediaDetails.license.wideVine,
                };
            }
            return DRM_Video_Details;
        }
        public async Task<string> Get_MoviFlex_AuthToken()
        {
            string App_ID = _appSettings.App_ID;
            string Secret_Key = _appSettings.Secret_Key;
            string base_URL = "https://apigateway.muvi.com/";

            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), base_URL + "get-user-token-details?secret_key=" + Secret_Key + "&app_id=" + App_ID))
                {
                    var response = await httpClient.SendAsync(request);
                    var response_data = response.Content.ReadAsStringAsync().Result;
                    var Response_Code = JsonConvert.DeserializeObject<dynamic>(response_data);

                    var Access_Token = Response_Code.response.access_token;
                    ViewBag.Access_Token = Access_Token;
                    return Access_Token;
                }
            }
        }
    }
}
