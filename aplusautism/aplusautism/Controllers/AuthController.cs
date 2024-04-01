using aplusautism.Service;
using aplusautism.Bal.DTO;
using aplusautism.Bal.ILogic;
using aplusautism.Data.Models;
using aplusautism.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Reflection.Metadata;
using aplusautism.Data;
using aplusautism.ExceptionHandler;
using aplusautism.Bal.Logic;
using Microsoft.Extensions.Options;
using aplusautism.Setting;

namespace aplusautism.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMailService _mailService;
        public AplusautismDbContext _dbcontext; 
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly MailSetting _mailSetting;
        protected IGlobalCodeCategorylogic _IGlobalCodeCategory { get; private set; }
        protected Iuserlogic _IuserLogicBAL { get; private set; }
        private RoleManager<IdentityRole> _roleManager
        {
            get;
        }
        public AuthController(UserManager<AppUser> userManager, IOptions<MailSetting> mailSetting, Iuserlogic iuserLogicBAL, IHttpContextAccessor httpContextAccessor, SignInManager<AppUser> signInManager, IMailService mailService, RoleManager<IdentityRole> roleManager, IGlobalCodeCategorylogic IGlobalCodeCategory, AplusautismDbContext dbcontext)
        {
            _mailSetting = mailSetting.Value;
            _IuserLogicBAL = iuserLogicBAL;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _IGlobalCodeCategory = IGlobalCodeCategory;
            _mailService = mailService;
            _dbcontext = dbcontext;
            _httpContextAccessor = httpContextAccessor;
        }


        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ForgetPassword()
        {
            return View();
        }

        public IActionResult EmailForgetMailSend(string Username)
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext;
            //var tomail = httpContext.Session.GetString("username");
            
            var frommail = _mailSetting.SenderEmail;
            ContactLogDTO contactLogDTO = new ContactLogDTO();
            try
            {
                var data = _IuserLogicBAL.ForgetPassword(Username);
                var passwodnew = data.FirstOrDefault().Password;
               // var usernew = data.FirstOrDefault().Username;

                var AB_MAINID = _dbcontext.Ab_main.Where(i => i.Email == Username).FirstOrDefault().AB_MainID;
                if (data != null && data.Count() > 0)
                {
                    // for getting Email Template for Forgot password
                    string startupPath = System.IO.Directory.GetCurrentDirectory();
                    startupPath = startupPath + "\\wwwroot\\EmailTemplates\\";
                    string ForgotTemplate = System.IO.File.ReadAllText(Path.Combine(startupPath, "ForgotPassword.html"));
                    ForgotTemplate = ForgotTemplate.Replace("@Username", Username);
                    ForgotTemplate = ForgotTemplate.Replace("@Password", passwodnew);

                    var result = _mailService.SendmailTest(frommail, Username, "Password Notification", "", ForgotTemplate);
                   // var result = _mailService.SendForgotPassword(Username.ToString(), ForgotTemplate);

                    //string EmailSubject = "Forget password";
                    //string MailMessage = "";
                    //MailMessage += " Hello " + data.FirstOrDefault().FirstName + " <br>It seems like you forgot your password to A+ Autism.com <br> We have this email on file for you and it is also your username for our site: " + Username + "<br> This is your  password:  " + data.FirstOrDefault().Password + " <br> Thanks, <br> The A+ Autism team. <br> aplusautism@gmail.com <br> https://www.aplusautism.com/<br> ";

                   // var result = _mailService.SendEmailAsync(Username, data.FirstOrDefault().Password, data.FirstOrDefault().FirstName, MailMessage);
                    if (result != null)
                    {
                        string ABUserID = HttpContext.Session.GetString("AB_UserID");
                        
                        contactLogDTO.AB_MAINID = Convert.ToInt64(AB_MAINID);
                        contactLogDTO.Subject = "Forgot Password email.";
                        contactLogDTO.Disposition = "This is informed that a forgot email is send to User : "+ Username + "";
                        contactLogDTO.ContactType = Convert.ToInt32(aplusautism.Common.Enums.ContactTypes.Forgetpassword).ToString();//"Forget password notification";
                        contactLogDTO.ContactTypeTopic = Convert.ToInt32(aplusautism.Common.Enums.ContactTypes.ContactTypeTopic).ToString();// " Forget password notification";
                        contactLogDTO.ContactMethod = Convert.ToInt32(aplusautism.Common.Enums.ContactTypes.ContactMethod).ToString();//"Forget password notification";
                        contactLogDTO.CreatedBy = Convert.ToInt64(ABUserID);
                        contactLogDTO.CreatedDate = DateTime.Now;
                        var status = _IuserLogicBAL.SaveContactLog(contactLogDTO);
                        return Json(true);

                    }
                    else
                    {
                        return Json(false);
                    }
                }
                else
                {
                    return Json(false);
                }

                return RedirectToAction("ForgetPassword");
            }
            catch (AppException ex)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
        //for sign view
        public IActionResult SignIn(LoginDTO login, string Registereduser)
        {
            ViewBag.registereduser = false;
            if (Registereduser == "yes")
            {

                ViewBag.registereduser = true;
            }

            if (Registereduser == "error")
            {

                ViewBag.Error = "Invalid User Name Or Password";
            }
            if(Registereduser == "extra")
            {
                ViewBag.Error = "You have looged in three device already";
            }
            if (Registereduser == "5")
            {
                ViewBag.Error = "Your account has been suspended.";
            }
            if(Registereduser == "error2")
            {
                ViewBag.Error = "Your account has been deleted";
            }
            return View();
        }

        private IActionResult RedirectToAction(string v1, string v2, object customer)
        {
            throw new NotImplementedException();
        }
        ////for sign
        //[HttpPost]
        //public async Task<IActionResult> SignIn(SignInVM signIn, string ReturnUrl)
        //{
        //    AppUser user;

        //    try
        //    {
        //        if (signIn.UsernameOrEmail.Contains("@"))
        //        {
        //            user = await _userManager.FindByEmailAsync(signIn.UsernameOrEmail);
        //        }
        //        else
        //        {
        //            user = await _userManager.FindByNameAsync(signIn.UsernameOrEmail);
        //        }
        //        if (user == null)
        //        {
        //            ModelState.AddModelError("", "Login ve ya parol yalnisdir");
        //            return View(signIn);
        //        }
        //        //var result = await
        //        //_signInManager.PasswordSignInAsync(user, signIn.Password, signIn.RememberMe, true);
        //        //if (!result.Succeeded)
        //        //{
        //        //    ModelState.AddModelError("", "Login ve ya parol yalnisdir");
        //        //    return View(signIn);
        //        //}

        //        if (ReturnUrl != null) return LocalRedirect(ReturnUrl);

        //        if (user != null)
        //        {
        //            await _signInManager.SignOutAsync();
        //            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, signIn.Password, signIn.RememberMe, false);
        //            if (result.Succeeded)
        //            {

        //                var claims = new List<Claim>
        //               {
        //           new Claim("username", user.UserName)

        //              };
        //                var userIdentity = new ClaimsIdentity();
        //                userIdentity.AddClaims(claims);

        //                ClaimsPrincipal userPrincipal = new ClaimsPrincipal(userIdentity);




        //                return RedirectToAction("Index", "Home", new  { email = user.Email });
        //            }
        //        }

        //        //return RedirectToAction("Index", "Team", new
        //        //{
        //        //    area = "admin"
        //        //});

        //        return RedirectToAction("Index", "Home");

        //    }
        //    catch(Exception ex)
        //    {
        //        return null;
        //    }
        //}


        //for register view
        public String Userexist(string isuserexist)
        {
            try
            {

                var isexituser = _IuserLogicBAL.GetLoginUserDetails(isuserexist, "2");
                if (isexituser.count > 0)
                {



                    return "yes";

                }
            }
            catch (AppException ex)
            {
                return null; ;
            }

            return "no";
        }

        public string KeepSessionAlive(string Check)
        {
            string TodayDate = DateTime.Now.ToString();
            try
            {
              
                return TodayDate;
            }
            catch (AppException ex)
            {
                return null; ;
            }

            return TodayDate;
        }


        public IActionResult Register(string isexitparameter)
        {
            try
            {
                ViewBag.UserExsit = false;
                var languages = _IGlobalCodeCategory.GetGlobalCodesByGlobalCodeCategoryValue("8", "2", "", "", "");
                var CountryList = _IGlobalCodeCategory.GetCountries().OrderBy(x => x.Name != "United States").ToList();
                RegisterUserDTO userDTO = new RegisterUserDTO();
                userDTO.LanguagesList = languages;
                userDTO.Countrieslist = CountryList;
                if (isexitparameter == "yes")
                {

                    ViewBag.UserExsit = true;
                }

                return View(userDTO);
            }
            catch (AppException ex)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }



        //get state

        public IActionResult GetStatelist(int CountryId)
        {
            var statelist = _IGlobalCodeCategory.Getstate(CountryId).ToList();
            return Ok(statelist);
        }

        //get city
        public IActionResult getcity(int stateid)
        {
            var citylist = _IGlobalCodeCategory.GetcityList(0, stateid).ToList();
            return Ok(citylist);
        }




        //register new user

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM register)
        {
            
                if (!ModelState.IsValid) return View();

                try
                {
                    AppUser newUser = new AppUser
                    {
                        Email = register.Email,
                        UserName = register.Username
                    };
                    IdentityResult result = await _userManager.CreateAsync(newUser, register.Password);
                    if (!result.Succeeded)
                    {
                        foreach (var item in result.Errors)
                        {
                            ModelState.AddModelError("", item.Description);
                        }
                    }
                    return RedirectToAction("SignIn");
                }
                catch (ApplicationException ex)
                {
                    return RedirectToAction("SignIn");
                }
            

        }

        //logout method
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(SignIn));
        }

    }
}
