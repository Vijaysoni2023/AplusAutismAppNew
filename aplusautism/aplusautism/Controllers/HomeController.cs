using aplusautism.Bal.ILogic;
using aplusautism.Data.Models;
using aplusautism.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace aplusautism.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SignInManager<AppUser> _signInManager;

      

        protected Iuserlogic _IuserLogicBAL { get; private set; }
        private readonly UserManager<AppUser> _userManager;
        public HomeController(
            ILogger<HomeController> logger,
            UserManager<AppUser> userMgr, 
            SignInManager<AppUser> signInManager,
            Iuserlogic IuserLogicBAL
     

            )
        {
            _logger = logger;
            _userManager = userMgr;
            _signInManager = signInManager;
            _IuserLogicBAL = IuserLogicBAL;
         
        }

       
        public async Task<IActionResult> Index(string email)
        {
            AppUser user;
            if (email.Contains("@"))
            {
                user = await _userManager.FindByEmailAsync(email);
            }
            else
            {
                user = await _userManager.FindByNameAsync(email);
            }


            var userdetails= _IuserLogicBAL.GetLoginUserDetails(user.Email,"1");

            string message = "Hello " + userdetails.UserName;
            ViewBag.username = message;
            return View(userdetails);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("login", "Auth");
        }
    }
}