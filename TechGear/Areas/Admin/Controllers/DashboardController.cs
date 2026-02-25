using Microsoft.AspNetCore.Mvc;
using TechGear.Constants;
using TechGear.Filters;

namespace TechGear.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthorizeRole(RoleConstants.Admin, RoleConstants.Staff)]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            ViewBag.UserRole = userRole;
            return View();
        }
    }
}
