using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechGear.Models;

namespace TechGear.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        [HttpGet]
        public IActionResult Index(string search, int page = 1, int pageSize = 10)
        {
            ViewBag.Search = search;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            return View();
        }

        public IActionResult Details(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product model)
        {
            if (!ModelState.IsValid) return View(model);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Product model)
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
