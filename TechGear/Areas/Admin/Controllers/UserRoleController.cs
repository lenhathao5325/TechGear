using TechGear.Models;
using TechGear.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace TechGear.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserRoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;

        public UserRoleController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }

        public async Task<IActionResult> Index()
        {
            var users = _userManager.Users.ToList();
            var model = new List<UserRoleVM>();

            foreach (var user in users)
            {
                model.Add(new UserRoleVM
                {
                    UserId = user.Id,
                    Email = user.Email,
                    Roles = await _userManager.GetRolesAsync(user)
                });
            }

            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ApplicationUser input, string password)
        {
            if (input == null || string.IsNullOrWhiteSpace(input.Email) || string.IsNullOrWhiteSpace(password))
            {
                ModelState.AddModelError(string.Empty, "Vui lòng nhập email và mật khẩu.");
                return View(input);
            }

            var user = new ApplicationUser { UserName = input.Email, Email = input.Email, FullName = input.FullName, EmailConfirmed = true };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(Index));
            }
            foreach (var err in result.Errors) ModelState.AddModelError(string.Empty, err.Description);
            return View(input);
        }
    }
}
