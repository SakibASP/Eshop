using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eshop.Web.Data;
using Eshop.Web.Helper;
using Eshop.Web.Models;
using Eshop.Web.Models.ViewModels;
using Eshop.Web.Common;
using Eshop.Web.Interfaces;
using Eshop.Models.BusinessDomains;
using Eshop.Utils;

namespace Eshop.Web.Controllers
{
    [Authorize]
    public class CartController(ApplicationDbContext context, IOrderProcessor orderProcessor) : BaseController<CartController>
    {
        public ViewResult Index(Cart cart,string returnUrl)
        {
            CartIndexViewModel cartIndex = new()
            {
                //Cart = GetCart(),
                Cart= cart,
                ReturnUrl = returnUrl,
            };
            //HttpContext.Session.SetInt32(Constant.CartTotal, cart.Lines.Sum(x => x.Quantity));
            //ViewData["CartTotal"] = HttpContext.Session.GetInt32(Constant.CartTotal);
            return View(cartIndex);

        }

        //adding new items to the cart
        public ActionResult AddToCart(Cart cart,int productId, string returnUrl)
        {
            //var cart = GetCart();
            Product? product = context.Products.Include(c=>c.Category1).FirstOrDefault(p => p.AutoId == productId);
            if (product != null)
            {
                try
                {
                    var _cart = cart.Lines.Where(x => x.Product.AutoId == product.AutoId).FirstOrDefault();
                    if (_cart != null && _cart.Quantity >= product.CurrentStock)
                    {
                        TempData["Error"] = "Sorry! You can not add more than available stock.";
                    }
                    else if(product.CurrentStock == 0)
                    {
                        TempData["Error"] = "Sorry! This product is out of stock.";
                    }
                    else
                    {
                        cart.AddItem(product, 1);
                        HttpContext.Session.SetInt32(Constant.CartTotal, cart.Lines.Sum(x => x.Quantity));
                        ViewData["CartTotal"] = HttpContext.Session.GetInt32(Constant.CartTotal);
                        SessionHelper.SetObjectAsJson<Cart>(HttpContext.Session, Constant.CART, cart);
                        TempData["Success"] = "Added to the cart successfully";
                    }
                }
                catch(Exception ex)
                {
                    TempData["Error"]="Error raised. The error is "+ex.Message;
                }
                
            }
            CartIndexViewModel cartIndex = new CartIndexViewModel
            {
                //Cart = GetCart(),
                Cart = cart,
                ReturnUrl = returnUrl,
            };
            //return RedirectToAction("Index","Product", new { returnUrl });
            return View(nameof(Index), cartIndex);
        }


        //removing an item from cart 
        public RedirectToActionResult RemoveFromCart(Cart cart,int productId, string returnUrl)
        {
            //var cart = GetCart();
            Product? product = context.Products.Include(c=>c.Category1).FirstOrDefault(p => p.AutoId == productId);
            if (product != null)
            {
                try
                {
                    cart.RemoveLine(product);

                    HttpContext.Session.SetInt32(Constant.CartTotal, cart.Lines.Sum(x => x.Quantity));
                    ViewData["CartTotal"] = HttpContext.Session.GetInt32(Constant.CartTotal);
                    SessionHelper.SetObjectAsJson<Cart>(HttpContext.Session, Constant.CART, cart);
                    TempData["Success"] = "Removed successfully";
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error raised. The error is " + ex.Message;
                }
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        // my total cart details
        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }

        //private Cart GetCart()
        //{
        //    //Cart cart = (Cart)Session[CART];
        //    Cart cart = SessionHelper.GetObjectFromJson<Cart>(HttpContext.Session, CART);
        //    if (cart == null)
        //    {
        //        cart = new Cart();
        //        //Session[CART] = cart;
        //        SessionHelper.SetObjectAsJson<Cart>(HttpContext.Session,CART,cart);
        //    }
        //    return cart;
        //}

        //Shipping Details
        public ViewResult Checkout(Cart cart)
        {
            ShippingDetails shippingDetails = new()
            { 
                IsConfirmed = false,
                CreatedBy = CurrentUserName,
                CreatedDate = DateTime.Today
            };
            if (cart.Lines.Count() == 0)
            {
                ViewData["EmptyCart"] = "True";
            }
            return View(shippingDetails);
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            ShipmentOrders shipmentOrders = new();
            if (!cart.Lines.Any())
            {
                ModelState.AddModelError("", "Sorry, your cart is empty!");               
            }
            if (ModelState.IsValid)
            {
                try
                {                   
                    await context.ShippingDetails.AddAsync(shippingDetails);
                    await context.SaveChangesAsync();
                    foreach(var i in cart.Lines)
                    {
                        shipmentOrders.AutoId = null;
                        shipmentOrders.ShippingDetailsId = shippingDetails.AutoId;
                        shipmentOrders.Quantity = i.Quantity;
                        shipmentOrders.ProductName = i.Product.Name;
                        shipmentOrders.ProductId = i.Product.AutoId;
                        shipmentOrders.CategoryId = i.Product.Cat_Id;
                        shipmentOrders.Price = (float)(i.Product.Price * i.Quantity);
                        shipmentOrders.CreatedBy = shippingDetails.CreatedBy;
                        shipmentOrders.CreatedDate = shippingDetails.CreatedDate;

                        await context.ShipmentOrders.AddAsync(shipmentOrders);
                        await context.SaveChangesAsync();
                    }
                    
                    await orderProcessor.ProcessOrder(cart, shippingDetails);
                    cart.Clear();

                    TempData["Success"] = "Mail has been sent successfully.";
                    HttpContext.Session.Remove(Constant.CART);
                    HttpContext.Session.Remove(Constant.CartTotal);
                    HttpContext.Session.Remove(Constant.PENDING_ORDERS);
                    
                    return View("Completed");
                }
                catch(Exception ex)
                {
                    TempData["Error"] = "Failed!Something went wrong! " + ex.Message;
                    return View(shippingDetails);
                }
            }
            else
            {
                return View(shippingDetails);
            }
        }
        protected override void Dispose(bool disposing)
        {
            context.Dispose();
            base.Dispose(disposing);
        }
    }
}

