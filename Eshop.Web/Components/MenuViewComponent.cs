using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Eshop.Web.Data;
using Eshop.Web.Helper;
using Eshop.Web.Models;
using Eshop.Utils;
using Eshop.Models.Menu;

namespace Eshop.Web.Components
{
    public class MenuViewComponent(ApplicationDbContext context, UserManager<ApplicationUser> userManager) : ViewComponent
    {
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            HttpContext.Session.Remove(Constant.Menu);
            var UserId = _userManager.GetUserId(HttpContext.User);
            var MenuList = _context.Database.SqlQueryRaw<DynamicMenuItem>(
                                "exec usp_GetMenuData @UserId",
                                new SqlParameter("UserId", UserId)).ToList();

            List<DynamicMenuItem>? Menu = null;
            var SessionMenu = HttpContext.Session.GetObjectFromJsonList<DynamicMenuItem>(Constant.Menu);
            if (SessionMenu != null)
            {
                Menu = (List<DynamicMenuItem>?)SessionMenu;
            }
            else
            {
                Menu = MenuList;
                HttpContext.Session.SetObjectAsJson<DynamicMenuItem>(Constant.Menu, MenuList);
            }
            return await Task.Run(() => View("_Menu", Menu));
            //return View("_Menu", Menu);
        }
    }
}
