using aplusautism.Data;
using aplusautism.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aplusautism.Areas.Admin.ViewComponentModel
{
    public class HeadermenuViewComponent:ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HeadermenuViewComponent(IHttpContextAccessor httpContextAccessor)
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
            

            //IEnumerable<tableRowClass> mc = await context.tableRows.ToListAsync();

            //return View("YourViewName", mc);
            return View("_Headermenu", userobj);
        }

    }
}
