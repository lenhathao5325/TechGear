using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechGear.Data;
using TechGear.Models;
using Microsoft.EntityFrameworkCore;

namespace TechGear.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search, int page = 1, int pageSize = 10)
        {
            var q = _db.Categories.AsQueryable();
            if (!string.IsNullOrWhiteSpace(search)) q = q.Where(c => c.CategoryName.Contains(search));
            var total = await q.CountAsync();
            var items = await q.OrderBy(c => c.CategoryName).Skip((page-1)*pageSize).Take(pageSize).ToListAsync();
            ViewBag.Search = search; ViewBag.Page = page; ViewBag.PageSize = pageSize; ViewBag.Total = total;
            return View(items);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.Categories.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var item = await _db.Categories.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category model)
        {
            var item = await _db.Categories.FindAsync(id);
            if (item == null) return NotFound();
            item.CategoryName = model.CategoryName;
            item.Description = model.Description;
            item.IsActive = model.IsActive;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var item = await _db.Categories.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _db.Categories.FindAsync(id);
            if (item != null)
            {
                _db.Categories.Remove(item);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
