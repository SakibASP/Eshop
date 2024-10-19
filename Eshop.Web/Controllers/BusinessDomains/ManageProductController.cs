using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Eshop.Web.Common;
using Eshop.Web.Data;
using Eshop.Web.Helper;
using Eshop.Web.Models;
using Eshop.Models.BusinessDomains;
using Eshop.ViewModels.BusinessDomains;
using Eshop.Utils;
using Eshop.Web.Controllers.Common;
//using X.PagedList;

namespace Eshop.Web.Controllers.BusinessDomains
{
    [Authorize]
    public class ManageProductController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment) : BaseController
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IWebHostEnvironment _webHostEnvironment = webHostEnvironment;

        // GET: SystemAdmin
        public async Task<ActionResult> Index(int? cat_id, string? sortOrder, string? currentFilter, string? searchString, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
                page = 1;
            else
                searchString = currentFilter;

            ViewData["CurrentFilter"] = searchString ?? "";

            List<Product>? products = await _context.Products.Include(x => x.Category).Take(100).ToListAsync();

            // From session
            if (cat_id != null)
                products = _context.Products.Where(c => c.CategoryId == cat_id).Take(100).OrderBy(s => s.Name).ToList();

            if (!string.IsNullOrEmpty(searchString))
                products = await _context.Products.Include(x => x.Category).Where(s => s.Name!.ToUpper().Contains(searchString.ToUpper()) || s.Category.CategoryName.ToUpper().Contains(searchString.ToUpper())).Take(100).ToListAsync();

            products = sortOrder switch
            {
                "name_desc" => products.OrderByDescending(s => s.Name).ToList(),
                // Name ascending 
                _ => products.OrderBy(s => s.Name).ToList(),
            };
            int pageSize = 6;
            int pageNumber = page ?? 1;

            ViewData["Cat_Id"] = new SelectList(_context.Category.AsNoTracking(), "AutoId", "CategoryName");
            //return View(products.ToPagedList(pageNumber, pageSize));
            var product_ = products.AsQueryable().AsNoTracking();
            return View(PaginatedList<Product>.CreateAsync(product_, pageNumber, pageSize));
        }

        // GET: SystemAdmin/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.ProductImages == null)
                return NotFound();

            List<ProductViewModel> product_mv = await Utility.GetProducts(_context, id, null, null, null);
            if (product_mv == null)
                return NotFound();

            return View(product_mv);
        }

        // GET: SystemAdmin/Create
        public IActionResult Create()
        {
            ViewData["Cat_Id"] = new SelectList(_context.Category, "AutoId", "CategoryName");
            ViewData["UnitId"] = new SelectList(_context.Units, "AutoId", "UnitName");
            return View();
        }

        // POST: SystemAdmin/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var imgFile = Request.Form.Files.FirstOrDefault();
                    if (imgFile != null)
                    {
                        product.ImageName = imgFile.FileName;
                    }
                    product.CreatedBy = CurrentUserName;
                    product.CreatedDate = DateTime.Now;
                    if (product.CurrentStock > 0)
                    {
                        product.IsAvailabe = true;
                    }

                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    await AddImagesAsync(product.AutoId, 1);

                    HttpContext.Session.Remove(Constant.PRODUCTS_LIST);
                    HttpContext.Session.Remove(Constant.TOTAL_PRODUCTS_LIST);
                    TempData["Success"] = "Added succesfully";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Failed! Something went wrong. Alert : " + ex.Message;
                }
            }
            ViewData["Cat_Id"] = new SelectList(_context.Category, "AutoId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // GET: SystemAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["Cat_Id"] = new SelectList(_context.Category, "AutoId", "CategoryName", product.CategoryId);
            return View(product);
        }

        // POST: SystemAdmin/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (id != product.AutoId)
                    {
                        return NotFound();
                    }
                    var img_file = Request.Form.Files.FirstOrDefault();
                    if (img_file != null)
                    {
                        product.ImageName = img_file.FileName;
                    }
                    if (product.CurrentStock > 0)
                    {
                        product.IsAvailabe = true;
                    }
                    else
                    {
                        product.IsAvailabe = false;
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();

                    HttpContext.Session.Remove(Constant.PRODUCTS_LIST);
                    HttpContext.Session.Remove(Constant.TOTAL_PRODUCTS_LIST);
                    TempData["Success"] = "Successfully updated";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ProductExists(product.AutoId))
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
            ViewData["Cat_Id"] = new SelectList(_context.Category, "AutoId", "CategoryName", product.CategoryId);
            return View(product);
        }
        // GET: SystemAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.AutoId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: SystemAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                var productImages = _context.ProductImages.Where(m => m.ProductId == id).ToList();
                if (!User.IsInRole("SuperAdmin"))
                {
                    if (product.CurrentStock > 0)
                    {
                        TempData["Error"] = "Sorry! This product is in stock. Please contact with Super Admin";
                    }
                    else
                    {
                        if (productImages != null) _context.ProductImages.RemoveRange(productImages);
                        _context.Products.Remove(product);

                        HttpContext.Session.Remove(Constant.PRODUCTS_LIST);
                        HttpContext.Session.Remove(Constant.TOTAL_PRODUCTS_LIST);
                        TempData["Success"] = string.Format("{0} ! Successfully deleted", product.Name);
                    }
                }
                else
                {
                    if (productImages != null) _context.ProductImages.RemoveRange(productImages);
                    _context.Products.Remove(product);

                    HttpContext.Session.Remove(Constant.PRODUCTS_LIST);
                    HttpContext.Session.Remove(Constant.TOTAL_PRODUCTS_LIST);
                    TempData["Success"] = string.Format("{0} ! Successfully deleted", product.Name);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //For adding photos to the image table
        public async Task<IActionResult> AddImagesAsync(int? ProductID, int? isCover)
        {
            try
            {
                ProductImages productImages = new();
                var img = Request.Form.Files.FirstOrDefault();
                var pRODUCT = await _context.Products.FindAsync(ProductID);
                if (img is not null && pRODUCT is not null)
                {
                    var catName = _context.Category.Find(pRODUCT.CategoryId)?.CategoryName ?? "Anonymous";
                    string? imagePath = catName + "\\" + GetImageNameWithExtension(img.FileName, pRODUCT.Name + "_" + pRODUCT.AutoId);
                    string? filePath = Path.Combine(_webHostEnvironment.WebRootPath, Constant.ImageFolderName, imagePath);

                    // Check if the directory exists; if not, create it
                    string? catPath = Path.Combine(_webHostEnvironment.WebRootPath, Constant.ImageFolderName, catName);
                    if (!Directory.Exists(catPath))
                        Directory.CreateDirectory(catPath);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                        await img.CopyToAsync(stream);


                    productImages.ProductId = ProductID;
                    productImages.IsCover = isCover;
                    productImages.CreatedBy = CurrentUserName;
                    productImages.CreatedDate = DateTime.Now;
                    productImages.ImageName = GetImageNameWithExtension(img.FileName, pRODUCT.Name + "_" + pRODUCT.AutoId);
                    productImages.ImagePath = "\\"+Path.Combine(Constant.ImageFolderName,imagePath);

                    await _context.AddAsync(productImages);
                    await _context.SaveChangesAsync();

                    HttpContext.Session.Remove(Constant.PRODUCTS_LIST);
                    HttpContext.Session.Remove(Constant.TOTAL_PRODUCTS_LIST);
                    TempData["Success"] = "Successfully added.";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                TempData["Error"] = "Failed! Something went wrong";
            }
            return RedirectToAction(nameof(Index));
        }

        //For removing stored photos in image tables
        public async Task<IActionResult> RemoveImage(int? id)
        {
            try
            {
                var productImages = await _context.ProductImages.FindAsync(id);
                if (productImages != null && !string.IsNullOrEmpty(productImages.ImagePath))
                {
                    // Construct the full file path to the image
                    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, productImages.ImagePath);

                    // Check if the image file exists
                    if (System.IO.File.Exists(imagePath))
                    {
                        // Delete the image file from the server's file system
                        System.IO.File.Delete(imagePath);
                        _context.Remove(productImages);
                        await _context.SaveChangesAsync();
                    }

                    HttpContext.Session.Remove(Constant.PRODUCTS_LIST);
                    HttpContext.Session.Remove(Constant.TOTAL_PRODUCTS_LIST);
                    TempData["Success"] = "Removed succesfully";
                }
                else
                {
                    TempData["Error"] = "Sorry! You have to delete the product.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed! Something went wrong";
            }
            return RedirectToAction(nameof(Index));
        }

        //For making cover
        public async Task<IActionResult> MakeCover(int? id)
        {
            try
            {
                var productImages = await _context.ProductImages.FindAsync(id);
                if (productImages != null)
                {
                    var CoverProductImage = _context.ProductImages.Where(p => p.ProductId == productImages.ProductId && p.IsCover == 1).ToList();
                    if (CoverProductImage.Count() > 0)
                    {
                        CoverProductImage.FirstOrDefault()!.IsCover = null;
                        _context.Update(productImages);
                        await _context.SaveChangesAsync();
                    }

                    productImages.IsCover = 1;
                    _context.Update(productImages);
                    await _context.SaveChangesAsync();

                    HttpContext.Session.Remove(Constant.PRODUCTS_LIST);
                    HttpContext.Session.Remove(Constant.TOTAL_PRODUCTS_LIST);
                    TempData["Success"] = "Cover setted succesfully";
                }
                else
                {
                    TempData["Error"] = "Sorry! You have to delete the product.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Failed! Something went wrong";
            }
            return RedirectToAction(nameof(Index));
        }

        private string? GetImageNameWithExtension(string fileName, string productName)
        {
            if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(productName))
            {
                return null;
            }
            int lastDotIndex = fileName.LastIndexOf('.');
            if (lastDotIndex >= 0)
            {
                fileName = productName!.Replace(" ", "") + fileName.Substring(lastDotIndex);
            }

            return fileName;
        }

        private async Task<bool> ProductExists(int id)
        {
            return (await _context.Products.AnyAsync(e => e.AutoId == id));
        }
        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
            base.Dispose(disposing);
        }
    }
}