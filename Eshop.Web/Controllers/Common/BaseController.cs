using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Eshop.Web.Data;
using Eshop.Web.Helper;
using System.Security.Claims;
using Eshop.ViewModels.BusinessDomains;
using Eshop.Models.BusinessDomains;
using Eshop.Utils;
using Eshop.Models.Menu;
using Eshop.Web.Common;

namespace Eshop.Web.Controllers.Common
{
    public abstract class BaseController : Controller
    {
        public string? CurrentUserId { get; set; }
        public string? CurrentUserName;
        public List<ProductViewModel>? S_PRODUCT_LIST;
        public List<Product>? S_TOTAL_PRODUCT_LIST;
        public decimal? S_SOCCER_PERCENTAGE;
        public decimal? S_CHESS_PERCENTAGE;
        public decimal? S_WATERSPORTS_PERCENTAGE;
        public decimal? S_CRICKET_PERCENTAGE;
        public int? S_PENDING_ORDERS;

        public override async Task OnActionExecutionAsync(ActionExecutingContext filterContext, ActionExecutionDelegate next)
        {
            //base.OnActionExecuting(filterContext);

            using ApplicationDbContext _context = new();
            base.OnActionExecuting(filterContext);
            if (filterContext.HttpContext.User.Identity!.IsAuthenticated)
            {
                CurrentUserName = filterContext.HttpContext.User.Identity.Name;
                CurrentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var CartSummary = HttpContext.Session.GetInt32(Constant.CartTotal);
                ViewData["CartTotal"] = CartSummary ?? 0;
            }

            // User Rights Handling
            var SessionMenu = HttpContext.Session.GetObjectFromJsonList<DynamicMenuItem>(Constant.Menu);
            if (SessionMenu != null)
            {
                var Menu = (IEnumerable<DynamicMenuItem>)SessionMenu;
                var path = Request.Path.ToString();
                if (path != "/" && path.Split('/')[1] != "Home" && path.Split('/')[1] != "Product" && path.Split('/')[1] != "Cart" && path.Split('/')[1] != "Payment")
                {
                    var rights = Menu.Where(h => h.MenuURL != "#").Where(p => p.MenuURL.Split('/')[1] == path.Split('/')[1]).ToList();
                    if (rights.Count() > 0)
                    {
                        //Do nothing
                    }
                    else
                    {
                        HttpContext.Session.Clear();
                        Request.Path = "/Identity/Account/Login";
                        HttpContext.Response.Redirect(Request.Path);
                    }

                }
            }

            //Store Stock Percentage to a Session
            var SessionSoccer = HttpContext.Session.GetInt32(Constant.SOCCER);
            var SessionWatersports = HttpContext.Session.GetInt32(Constant.WATERSPORTS);
            var SessionChess = HttpContext.Session.GetInt32(Constant.CHESS);
            var SessionCricket = HttpContext.Session.GetInt32(Constant.CRICKET);
            if (SessionSoccer != null || SessionWatersports != null || SessionChess != null || SessionCricket != null)
            {
                S_SOCCER_PERCENTAGE = Convert.ToDecimal(SessionSoccer);
                S_CHESS_PERCENTAGE = Convert.ToDecimal(SessionChess);
                S_WATERSPORTS_PERCENTAGE = Convert.ToDecimal(SessionWatersports);
                S_CRICKET_PERCENTAGE = Convert.ToDecimal(SessionCricket);
            }
            else
            {
                decimal? SoccerPercentage;
                decimal? WatersportsPercentage;
                decimal? ChessPercentage;
                decimal? CricketPercentage;
                Utility.GetProductStock(out SoccerPercentage, out WatersportsPercentage, out ChessPercentage, out CricketPercentage, _context);
                S_SOCCER_PERCENTAGE = SoccerPercentage;
                S_CHESS_PERCENTAGE = ChessPercentage;
                S_WATERSPORTS_PERCENTAGE = WatersportsPercentage;
                S_CRICKET_PERCENTAGE = CricketPercentage;

                HttpContext.Session.SetInt32(Constant.SOCCER, Convert.ToInt32(S_SOCCER_PERCENTAGE));
                HttpContext.Session.SetInt32(Constant.WATERSPORTS, Convert.ToInt32(S_WATERSPORTS_PERCENTAGE));
                HttpContext.Session.SetInt32(Constant.CHESS, Convert.ToInt32(S_CHESS_PERCENTAGE));
                HttpContext.Session.SetInt32(Constant.CRICKET, Convert.ToInt32(S_CRICKET_PERCENTAGE));
            }

            if (CurrentUserId != null)
            {
                //Store Pending Orders to a Session
                var orderPending = HttpContext.Session.GetInt32(Constant.PENDING_ORDERS);
                if (orderPending != null)
                {
                    S_PENDING_ORDERS = orderPending;
                    ViewData["OrdersPending"] = S_PENDING_ORDERS;
                }
                else
                {
                    int? _pendingOrders = 0;
                    Utility.GetPendingOrders(_context, out _pendingOrders);
                    S_PENDING_ORDERS = _pendingOrders;
                    ViewData["OrdersPending"] = S_PENDING_ORDERS;
                    HttpContext.Session.SetInt32(Constant.PENDING_ORDERS, Convert.ToInt32(S_PENDING_ORDERS));
                }
            }
            // Call the next action in the pipeline
            await next();
        }
    }
}
