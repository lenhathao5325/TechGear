using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechGear.Data;
using TechGear.Models;
using Microsoft.EntityFrameworkCore;

namespace TechGear.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class BrandController : Controller
    {
        private readonly ApplicationDbContext _db;
        public BrandController(ApplicationDbContext db)
        {
            _db = db;
        }

        // Index with search, filter and paging
        [HttpGet]
        public async Task<IActionResult> Index(string search, string status, int page = 1, int pageSize = 10)
        {
            var query = _db.Brands.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(b => b.BrandName.Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                if (status == "active") query = query.Where(b => b.IsActive);
                else if (status == "inactive") query = query.Where(b => !b.IsActive);
            }

            var total = await query.CountAsync();
            var totalPages = (int)Math.Ceiling(total / (double)pageSize) + 1; // Adjusted to include the last page
            if (page < 1) page = 1;
            if (page > totalPages && totalPages > 0) page = totalPages;

            var items = await query
                .OrderBy(b => b.BrandName)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.Search = search;
            ViewBag.Status = status;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalPages = totalPages;
            ViewBag.TotalItems = total;

            return View(items);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Brand model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.Brands.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.Brands.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Brand model)
        {
            var item = await _db.Brands.FindAsync(id);
            if (item == null) return NotFound();
            item.BrandName = model.BrandName;
            item.Description = model.Description;
            item.LogoUrl = model.LogoUrl;
            item.IsActive = model.IsActive;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Brands.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _db.Brands.FindAsync(id);
            if (item != null)
            {
                _db.Brands.Remove(item);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
