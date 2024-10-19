using Microsoft.AspNetCore.Mvc;

namespace Eshop.Web.Controllers.Common
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
