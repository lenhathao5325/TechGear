using Microsoft.AspNetCore.Mvc;
using TechGear.Constants;
using TechGear.Filters;
using TechGear.Services;

namespace TechGear.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthorizeRole(RoleConstants.Admin)] // Chỉ Admin mới được truy cập
    public class AdminOrdersController : Controller
    {
        private readonly IApiService _apiService;

        public AdminOrdersController(IApiService apiService)
        {
            _apiService = apiService;
        }

        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JWToken");
            // TODO: Implement get all orders from API
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");
            // TODO: Implement get order details from API
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var token = HttpContext.Session.GetString("JWToken");
            // TODO: Implement update order status API call
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
