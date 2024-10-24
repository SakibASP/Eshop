﻿using iText.Html2pdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Eshop.Web.Data;
using Eshop.Utils;
using Eshop.Models.BusinessDomains;
using Eshop.Web.Controllers.Common;

namespace Eshop.Web.Controllers.BusinessDomains
{
    public class ShippingDetailsController(ApplicationDbContext context) : BaseController
    {
        private readonly ApplicationDbContext _context = context;

        // GET: ShippingDetails
        public async Task<IActionResult> Index()
        {
            return View(await _context.ShippingDetails.ToListAsync());
        }
        public async Task<IActionResult> PendingIndex()
        {
            return View(await _context.ShippingDetails.Where(x => x.IsConfirmed == false).ToListAsync());
        }

        public async Task<IActionResult> CofirmOrder(int? id)
        {
            if (id == null || _context.ShippingDetails == null)
            {
                return NotFound();
            }

            var shippingDetails = await _context.ShippingDetails
                .FirstOrDefaultAsync(m => m.AutoId == id);
            if (shippingDetails == null)
            {
                return NotFound();
            }
            else
            {
                shippingDetails.IsConfirmed = true;
                shippingDetails.ConfirmBy = CurrentUserName;
                shippingDetails.ConfirmDate = DateTime.Today;
                UpDateProduct(id);

                _context.ShippingDetails.Update(shippingDetails);
                _context.SaveChanges();

                TempData["Success"] = "Order confirmed successfully !";
                HttpContext.Session.Remove(Constant.PENDING_ORDERS);
                HttpContext.Session.Remove(Constant.PRODUCTS_LIST);
                HttpContext.Session.Remove(Constant.TOTAL_PRODUCTS_LIST);
            }
            return RedirectToAction(nameof(PendingIndex));
        }

        private void UpDateProduct(int? id)
        {
            var shipmentOrder = _context.ShipmentOrders.Where(x => x.ShippingDetailsId == id).ToList();
            foreach (var i in shipmentOrder)
            {
                var Product_ = _context.Products.Where(x => x.AutoId == i.ProductId).FirstOrDefault();
                Product_.CurrentStock = Product_.CurrentStock - i.Quantity;
                _context.Products.Update(Product_);
            }
        }

        // GET: ShippingDetails/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ShipmentOrders == null)
            {
                return NotFound();
            }
            var shippingOrders = await _context.ShipmentOrders
                .Include(x => x._ShippingDetails)
                .Where(m => m.ShippingDetailsId == id).ToListAsync();
            if (shippingOrders == null)
            {
                return NotFound();
            }

            return View(shippingOrders);
        }

        public async Task<IActionResult> ViewDetails(int? id)
        {
            if (id == null || _context.ShipmentOrders == null)
            {
                return NotFound();
            }
            var shippingOrders = await _context.ShipmentOrders
                .Include(x => x._ShippingDetails)
                .Where(m => m.ShippingDetailsId == id).ToListAsync();
            if (shippingOrders == null)
            {
                return NotFound();
            }

            return View(shippingOrders);
        }

        // GET: ShippingDetails/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ShippingDetails/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ShippingDetails shippingDetails)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shippingDetails);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shippingDetails);
        }

        // GET: ShippingDetails/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.ShippingDetails == null)
            {
                return NotFound();
            }

            var shippingDetails = await _context.ShippingDetails.FindAsync(id);
            if (shippingDetails == null)
            {
                return NotFound();
            }
            return View(shippingDetails);
        }

        // POST: ShippingDetails/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ShippingDetails shippingDetails)
        {
            if (id != shippingDetails.AutoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shippingDetails);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShippingDetailsExists(shippingDetails.AutoId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(shippingDetails);
        }

        // GET: ShippingDetails/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.ShippingDetails == null)
            {
                return NotFound();
            }

            var shippingDetails = await _context.ShippingDetails
                .FirstOrDefaultAsync(m => m.AutoId == id);
            if (shippingDetails == null)
            {
                return NotFound();
            }

            return View(shippingDetails);
        }

        // POST: ShippingDetails/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.ShippingDetails == null)
            {
                return Problem("Entity set 'ApplicationDbContext.ShippingDetails'  is null.");
            }
            var shippingDetails = await _context.ShippingDetails.FindAsync(id);
            var shipmentOrders = await _context.ShipmentOrders.Where(x => x.ShippingDetailsId == id).ToListAsync();
            if (shippingDetails != null)
            {
                _context.ShipmentOrders.RemoveRange(shipmentOrders);
                _context.ShippingDetails.Remove(shippingDetails);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Export(string GridHtml)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                HtmlConverter.ConvertToPdf(GridHtml, stream);
                return File(stream.ToArray(), "application/pdf");
            }
        }
        private bool ShippingDetailsExists(int id)
        {
            return _context.ShippingDetails.Any(e => e.AutoId == id);
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}
