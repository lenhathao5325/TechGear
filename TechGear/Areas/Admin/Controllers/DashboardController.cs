using Microsoft.AspNetCore.Mvc;
using TechGear.Data;
using Microsoft.EntityFrameworkCore;
using TechGear.Models.ViewModels;


namespace TechGear.Areas.Admin.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _db;
        public DashboardController(ApplicationDbContext db)
        {
            _db = db;
        }

        [Area("Admin")]
        public async Task<IActionResult> Index()
        {
            var vm = new AdminDashboardVM();

            vm.TotalSales = await _db.Orders.SumAsync(o => (decimal?)o.TotalAmount) ?? 0;
            vm.OrdersCount = await _db.Orders.CountAsync();
            vm.ProductsCount = await _db.Products.CountAsync();
            vm.UsersCount = await _db.Users.CountAsync();

            var recent = await _db.Orders.OrderByDescending(o => o.OrderDate).Take(5).Include(o => o.User).ToListAsync();
            vm.RecentOrders = recent.Select(o => new RecentOrderItem{ OrderId = o.OrderId, UserName = o.User?.UserName ?? "-", Total = o.TotalAmount, Status = o.Status.ToString() }).ToList();

            // low stock: pick variants with stock <=5
            var low = await _db.ProductVariants.OrderBy(v => v.Stock).Take(5).ToListAsync();
            vm.LowStockProducts = low.Select(l => new LowStockItem{ Name = l.Product?.Name ?? l.SKU, Left = l.Stock }).ToList();

            return View(vm);
        }
    }
}
