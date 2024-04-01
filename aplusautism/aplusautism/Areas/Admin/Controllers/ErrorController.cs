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
    public class ErrorController : Controller
    {

        // for getting file Path
        private IHostingEnvironment _hostingEnvironment;
        protected Iclientlogic _Iclientlogic { get; private set; }

        public IlessondetailsBycategory _IlessondetailsBycategory { get; private set; }
        public IWebHostEnvironment Environment { get; private set; }
        protected Iuserlogic _IuserLogicBAL { get; private set; }


        protected ISubscriptionsetup _ISubscriptionsetup { get; private set; }
        protected IGlobalCodeCategorylogic _IGlobalCodeCategory { get; private set; }
        public Iadmindashboard _Iadmindashboard { get; private set; }
        private readonly IMailService _mailService;
        private readonly MailSetting _mailSetting;

        private readonly IHttpContextAccessor _httpContextAccessor;

        public AplusautismDbContext _dbcontext;


        public ErrorController(Iuserlogic iuserLogicBAL, IOptions<MailSetting> mailSetting, Iadmindashboard iadmindashboard,
            IGlobalCodeCategorylogic IGlobalCodeCategory,
            IMailService mailService, IHostingEnvironment hostingEnvironment,
            IWebHostEnvironment _environment, ISubscriptionsetup iSubscriptionsetup,
            IHttpContextAccessor httpContextAccessor, Iclientlogic iclientlogic, AplusautismDbContext dbcontext, IlessondetailsBycategory IlessondetailsBycategory
            )
        {
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
            ViewBag.ErrorMessage = "Your session has been expired , Please login again";
            return View();
        }


    }
}

