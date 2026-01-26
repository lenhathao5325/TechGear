using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechGear.Data;
using TechGear.Models;

namespace TechGear.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;
        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search, int page = 1, int pageSize = 10)
        {
            var q = _db.Products.Include(p => p.Brand).Include(p => p.Category).AsQueryable();
            if (!string.IsNullOrWhiteSpace(search)) q = q.Where(p => p.Name.Contains(search));
            var total = await q.CountAsync();
            var items = await q.OrderBy(p => p.Name).Skip((page-1)*pageSize).Take(pageSize).ToListAsync();
            ViewBag.Search = search; ViewBag.Page = page; ViewBag.PageSize = pageSize; ViewBag.Total = total;
            return View(items);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _db.Products
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.ProductVariants)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            ViewBag.Brands = await _db.Brands.ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _db.Categories.ToListAsync();
                ViewBag.Brands = await _db.Brands.ToListAsync();
                return View(model);
            }

            _db.Products.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();
            ViewBag.Categories = await _db.Categories.ToListAsync();
            ViewBag.Brands = await _db.Brands.ToListAsync();
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product model)
        {
            var product = await _db.Products.FindAsync(id);
            if (product == null) return NotFound();

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = await _db.Categories.ToListAsync();
                ViewBag.Brands = await _db.Brands.ToListAsync();
                return View(model);
            }

            product.Name = model.Name;
            product.Description = model.Description;
            product.CategoryId = model.CategoryId;
            product.BrandId = model.BrandId;
            product.ProductImages = model.ProductImages;

            _db.Products.Update(product);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var product = await _db.Products.Include(p => p.Brand).Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null) return NotFound();
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if (product != null)
            {
                _db.Products.Remove(product);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
