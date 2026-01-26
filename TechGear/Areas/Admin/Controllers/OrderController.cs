using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechGear.Data;
using TechGear.Models;

namespace TechGear.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _db;
        public OrderController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string search, string status, int page = 1, int pageSize = 10)
        {
            var q = _db.Orders.Include(o => o.User).AsQueryable();
            if (!string.IsNullOrWhiteSpace(search)) q = q.Where(o => o.OrderId.ToString().Contains(search) || o.User.Email.Contains(search));
            if (!string.IsNullOrWhiteSpace(status))
            {
                // parse status string to enum
                if (System.Enum.TryParse(typeof(TechGear.Models.Enums.Enums.OrderStatus), status, true, out var parsed))
                {
                    var s = (TechGear.Models.Enums.Enums.OrderStatus)parsed;
                    q = q.Where(o => o.Status == s);
                }
            }
            var total = await q.CountAsync();
            var totalPages = (int)Math.Ceiling(total / (double)pageSize);
            var items = await q.OrderByDescending(o => o.OrderId).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            ViewBag.Search = search; ViewBag.Status = status; ViewBag.Page = page; ViewBag.PageSize = pageSize; ViewBag.TotalPages = totalPages; ViewBag.Total = total;
            return View(items);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _db.Orders.Include(o => o.OrderDetails).ThenInclude(d => d.ProductVariant).ThenInclude(v => v.Product).FirstOrDefaultAsync(o => o.OrderId == id);
            if (order == null) return NotFound();
            return View(order);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order == null) return NotFound();
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Order model)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order == null) return NotFound();

            order.Status = model.Status;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order == null) return NotFound();
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _db.Orders.FindAsync(id);
            if (order != null)
            {
                _db.Orders.Remove(order);
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
