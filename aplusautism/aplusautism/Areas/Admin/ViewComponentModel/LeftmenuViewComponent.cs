using aplusautism.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace aplusautism.Areas.Admin.ViewComponentModel
{
    public class LeftmenuViewComponent : ViewComponent
    {
        private DbContextOptions<AplusautismDbContext> db = new DbContextOptions<AplusautismDbContext>();
        public async Task<IViewComponentResult> InvokeAsync()
        {
            AplusautismDbContext context = new AplusautismDbContext(db);
            //IEnumerable<tableRowClass> mc = await context.tableRows.ToListAsync();

            //return View("YourViewName", mc);
            return View("_LeftMenu");
        }
    }
}
