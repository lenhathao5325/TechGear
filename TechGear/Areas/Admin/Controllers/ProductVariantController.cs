using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace TechGear.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductVariantController : Controller
    {
        public IActionResult Index(int? productId)
        {
            ViewData["ProductId"] = productId;
            return View();
        }

        public IActionResult Create(int? productId)
        {
            ViewData["ProductId"] = productId;
            return View();
        }

        [HttpPost]
        public IActionResult Create(IFormCollection form)
        {
            // UI-only: handle create visually, then redirect back to index
            int.TryParse(form["ProductId"], out var pid);
            return RedirectToAction("Index", new { productId = pid });
        }

        public IActionResult Edit(int id)
        {
            ViewData["VariantId"] = id;
            return View();
        }

        [HttpPost]
        public IActionResult Edit(int id, IFormCollection form)
        {
            return RedirectToAction("Index");
        }

        public IActionResult Details(int id)
        {
            ViewData["VariantId"] = id;
            return View();
        }

        public IActionResult Delete(int id)
        {
            ViewData["VariantId"] = id;
            return View();
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            return RedirectToAction("Index");
        }
    }
}
