using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace TechGear.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserRoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(object input)
        {
            if (!ModelState.IsValid) return View(input);
            return RedirectToAction(nameof(Index));
        }
    }
}
