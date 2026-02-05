using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechGear.Models;

namespace TechGear.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ComboController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Combo model)
        {
            if (!ModelState.IsValid) return View(model);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Combo model)
        {
            if (!ModelState.IsValid) return View(model);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
