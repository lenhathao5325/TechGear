using TechGear.Models;
using TechGear.Models.ViewModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace TechGear.Areas.Admin.Controllers
{















    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class RoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(object model)
        {
            if (!ModelState.IsValid) return View(model);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, object model)
        {
            if (!ModelState.IsValid) return View(model);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(string id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            return RedirectToAction(nameof(Index));
        }
    }
}
