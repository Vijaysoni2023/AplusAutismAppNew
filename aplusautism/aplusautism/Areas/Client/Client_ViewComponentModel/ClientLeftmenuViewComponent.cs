using aplusautism.Bal.ILogic;
using aplusautism.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aplusautism.Areas.Client.Client_ViewComponentModel
{
    public class ClientLeftmenuViewComponent : ViewComponent
    {
        private DbContextOptions<AplusautismDbContext> db = new DbContextOptions<AplusautismDbContext>();
        public IlessondetailsBycategory _IlessondetailsBycategory { get; private set; }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            AplusautismDbContext context = new AplusautismDbContext(db);
            //IEnumerable<tableRowClass> mc = await context.tableRows.ToListAsync();

 
            //return View("YourViewName", mc);
            return View("_ClientLeftMenu");
        }
    }
}
