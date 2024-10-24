using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Eshop.Web.Data;
using Eshop.Web.Helper;
using Eshop.Web.Models;
using Eshop.Utils;
using Eshop.Models.Menu;
using Eshop.Interfaces.Common;

namespace Eshop.Web.Components
{
    public class MenuViewComponent(IMenuRepo menu,ApplicationDbContext context, UserManager<ApplicationUser> userManager) : ViewComponent
    {
        private readonly IMenuRepo _menu = menu;
        private readonly ApplicationDbContext _context = context;
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<IViewComponentResult> InvokeAsync()
        {
            HttpContext.Session.Remove(Constant.Menu);
            var userId = _userManager.GetUserId(HttpContext.User);
            var menuList = await _menu.GetAllMenuAsync(userId);

            List<DynamicMenuItem>? _menuList;
            var SessionMenu = HttpContext.Session.GetObjectFromJsonList<DynamicMenuItem>(Constant.Menu);
            if (SessionMenu != null)
            {
                _menuList = (List<DynamicMenuItem>?)SessionMenu;
            }
            else
            {
                _menuList = [.. menuList];
                HttpContext.Session.SetObjectAsJson<DynamicMenuItem>(Constant.Menu, menuList);
            }
            return View("_Menu", _menuList);
        }
    }
}
