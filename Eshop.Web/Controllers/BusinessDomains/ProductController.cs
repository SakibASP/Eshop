﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Eshop.Web.Common;
using Eshop.Web.Data;
using Eshop.Web.Helper;
using Eshop.ViewModels.BusinessDomains;
using Eshop.Web.Controllers.Common;


namespace Eshop.Web.Controllers.BusinessDomains
{
    public class ProductController(ApplicationDbContext context) : BaseController
    {
        private readonly ApplicationDbContext _context = context;

        // GET: Product
        //image/jpeg
        public async Task<IActionResult> Index(int? categoryId, int? price, string? sortOrder, string? currentFilter, string? searchString, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString ?? "";
            ViewData["Price"] = price;

            List<ProductViewModel> allProducts = await Utility.GetProducts(_context, null, categoryId, price, searchString);
            var coveredProduct = allProducts.Where(p => p.IsCover == 1).Distinct();
            var noCoveredProduct = allProducts.Where(p => p.ImagePath is null && !coveredProduct.Any(c => c.ProductId == p.ProductId)).Distinct();
            List<ProductViewModel> product_mv = [.. coveredProduct, .. noCoveredProduct];
            product_mv = sortOrder switch
            {
                "name_desc" => [.. product_mv.OrderByDescending(s => s.Name)],
                _ => [.. product_mv.OrderByDescending(x => x.CreatedDate)],
            };
            int pageSize = 6;
            int pageNumber = page ?? 1;

            ViewData["CategoryId"] = new SelectList(_context.Category, "AutoId", "CategoryName");

            var product_vm = product_mv.AsQueryable().AsNoTracking();
            return View(PaginatedList<ProductViewModel>.CreateAsync(product_vm, pageNumber, pageSize));
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductImages == null)
            {
                return NotFound();
            }

            List<ProductViewModel> product_mv = await Utility.GetProducts(_context, id, null, null, null);

            if (product_mv == null)
            {
                return NotFound();
            }

            return View(product_mv);
        }
    }
}
