using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechGear.Data;
using TechGear.Models;

namespace TechGear.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ComboController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ComboController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            var combos = await _db.Combos.Include(c => c.ComboItems).ThenInclude(ci => ci.ProductVariant).ThenInclude(v => v.Product).ToListAsync();
            return View(combos);
        }

        public async Task<IActionResult> Details(int id)
        {
            var combo = await _db.Combos
                .Include(c => c.ComboItems)
                .ThenInclude(ci => ci.ProductVariant)
                .ThenInclude(v => v.Product)
                .FirstOrDefaultAsync(c => c.ComboId == id);
            if (combo == null) return NotFound();
            return View(combo);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Variants = await _db.ProductVariants.Include(v => v.Product).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Combo model, int[] selectedVariantIds, int[] quantities)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Variants = await _db.ProductVariants.Include(v => v.Product).ToListAsync();
                return View(model);
            }

            model.CreatedAt = DateTime.Now;
            _db.Combos.Add(model);
            await _db.SaveChangesAsync();

            if (selectedVariantIds != null && selectedVariantIds.Length > 0)
            {
                for (int i = 0; i < selectedVariantIds.Length; i++)
                {
                    var vid = selectedVariantIds[i];
                    var qty = (quantities != null && quantities.Length > i) ? quantities[i] : 1;
                    var item = new ComboItem { ComboId = model.ComboId, ProductVariantId = vid, Quantity = qty };
                    _db.ComboItems.Add(item);
                }
                await _db.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var combo = await _db.Combos.Include(c => c.ComboItems).FirstOrDefaultAsync(c => c.ComboId == id);
            if (combo == null) return NotFound();
            ViewBag.Variants = await _db.ProductVariants.Include(v => v.Product).ToListAsync();
            return View(combo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Combo model, int[] selectedVariantIds, int[] quantities)
        {
            if (id != model.ComboId) return BadRequest();
            if (!ModelState.IsValid)
            {
                ViewBag.Variants = await _db.ProductVariants.Include(v => v.Product).ToListAsync();
                return View(model);
            }

            var combo = await _db.Combos.Include(c => c.ComboItems).FirstOrDefaultAsync(c => c.ComboId == id);
            if (combo == null) return NotFound();

            combo.ComboName = model.ComboName;
            combo.Description = model.Description;
            combo.OriginalPrice = model.OriginalPrice;
            combo.FinalPrice = model.FinalPrice;
            combo.DiscountType = model.DiscountType;
            combo.DiscountValue = model.DiscountValue;
            combo.IsActive = model.IsActive;

            _db.ComboItems.RemoveRange(combo.ComboItems);
            if (selectedVariantIds != null && selectedVariantIds.Length > 0)
            {
                for (int i = 0; i < selectedVariantIds.Length; i++)
                {
                    var vid = selectedVariantIds[i];
                    var qty = (quantities != null && quantities.Length > i) ? quantities[i] : 1;
                    _db.ComboItems.Add(new ComboItem { ComboId = combo.ComboId, ProductVariantId = vid, Quantity = qty });
                }
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var combo = await _db.Combos.FindAsync(id);
            if (combo == null) return NotFound();
            return View(combo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var combo = await _db.Combos.Include(c => c.ComboItems).FirstOrDefaultAsync(c => c.ComboId == id);
            if (combo != null)
            {
                _db.ComboItems.RemoveRange(combo.ComboItems);
                _db.Combos.Remove(combo);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
