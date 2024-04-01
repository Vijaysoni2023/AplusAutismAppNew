using aplusautism.Data;
using aplusautism.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aplusautism.Areas.Client.Client_ViewComponentModel
{
    public class ClientHeadermenuViewComponent : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientHeadermenuViewComponent(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private DbContextOptions<AplusautismDbContext> db = new DbContextOptions<AplusautismDbContext>();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            AplusautismDbContext context = new AplusautismDbContext(db);

            HttpContext httpContext = _httpContextAccessor.HttpContext;
            string Logedinuser = httpContext.Session.GetString("username");

            UserModel userobj = new UserModel();
            userobj.username = Logedinuser;
            userobj.UserFullName = httpContext.Session.GetString("UserFullName").ToString();
            return View("_ClientHeadermenu", userobj);
        }
    }

}
