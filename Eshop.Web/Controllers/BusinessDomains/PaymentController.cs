using Braintree;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Eshop.Web.Data;
using Eshop.Web.Helper;
using Eshop.Web.Interfaces;
using Eshop.Web.Models;
using Eshop.Web.Models.ViewModels;
using Eshop.Utils;
using Eshop.Web.Controllers.Common;

namespace Eshop.Web.Controllers.BusinessDomains
{
    public class PaymentController : BaseController
    {
        private readonly IBraintreeService _braintreeService;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public PaymentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IBraintreeService braintreeService)
        {
            _userManager = userManager;
            _context = context;
            _braintreeService = braintreeService;
        }
        public IActionResult Index(Cart cart)
        {
            var gateway = _braintreeService.GetGateway();
            var clientToken = gateway.ClientToken.Generate();  //Genarate a token
            ViewData["ClientToken"] = clientToken;

            CartIndexViewModel cartIndex = new CartIndexViewModel
            {
                Cart = cart
            };
            return View(cartIndex);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CartIndexViewModel model)
        {
            var usr = await _userManager.GetUserAsync(HttpContext.User);
            var gateway = _braintreeService.GetGateway();
            var customer = new CustomerRequest
            {
                FirstName = usr?.FirstName,
                LastName = usr?.LastName,
                Email = usr?.Email,
                Id = usr?.Id,
                Phone = usr?.PhoneNumber
            };
            var request = new TransactionRequest
            {
                Amount = Convert.ToDecimal(model.Cart.ComputeTotalValue()),
                PaymentMethodNonce = model.Nonce,
                Customer = customer,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(request);

            if (result.IsSuccess())
            {
                model.Cart.Clear();
                HttpContext.Session.SetInt32(Constant.CartTotal, model.Cart.Lines.Sum(x => x.Quantity));
                ViewData["CartTotal"] = HttpContext.Session.GetInt32(Constant.CartTotal);
                HttpContext.Session.SetObjectAsJson<Cart>(Constant.CART, model.Cart);

                return await Task.FromResult(View("Success"));
            }
            else
            {
                TempData["Error"] = "Failed! Something went wrong.";
                return await Task.FromResult(View("Index", model));
            }
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}
