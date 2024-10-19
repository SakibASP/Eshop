using Microsoft.AspNetCore.Mvc;
using Eshop.Web.Data;
using Eshop.Web.Models;
using System.Data;
using System.Diagnostics;
using Eshop.Utils;

namespace Eshop.Web.Controllers.Common
{
    public class HomeController(ApplicationDbContext context) : BaseController
    {
        public IActionResult Index()
        {
            ViewData["Soccer"] = Convert.ToInt32(S_SOCCER_PERCENTAGE);
            ViewData["Watersports"] = Convert.ToInt32(S_WATERSPORTS_PERCENTAGE);
            ViewData["Chess"] = Convert.ToInt32(S_CHESS_PERCENTAGE);
            ViewData["Cricket"] = Convert.ToInt32(S_CRICKET_PERCENTAGE);

            // Trying Raw sql query
            //var prod = RawSqlQuery.GetDynamicSqlValue(SP.rawProducts);

            return View();
        }


        public IActionResult Privacy()
        {
            HttpContext.Session.SetString(Constant.SessionTest, "This page is under development.");
            ViewBag.Message = HttpContext.Session.GetString(Constant.SessionTest);
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }
    }
}