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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _config;

        public RoleController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }

        // Danh sách USER
        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        // Gán quyền cho USER
        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var userRoles = await _userManager.GetRolesAsync(user);

            ViewBag.Roles = _roleManager.Roles.ToList();
            ViewBag.UserRoles = userRoles;

            // Only allow managing the Admin role if current operator matches the protected admin configured in appsettings
            var currentUser = await _userManager.GetUserAsync(User);
            var protectedAdminEmail = _config["AdminUser:Email"] ?? string.Empty;
            ViewBag.CanManageAdminRole = string.Equals(currentUser?.Email, protectedAdminEmail, StringComparison.OrdinalIgnoreCase);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string userId, List<string> roles)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            var currentRoles = await _userManager.GetRolesAsync(user);

            // Determine whether current operator can manage Admin role
            var currentUser = await _userManager.GetUserAsync(User);
            var protectedAdminEmail = _config["AdminUser:Email"] ?? string.Empty;
            var canManageAdminRole = string.Equals(currentUser?.Email, protectedAdminEmail, StringComparison.OrdinalIgnoreCase);

            // Normalize desired roles
            var desiredRoles = roles != null ? new List<string>(roles) : new List<string>();

            // If operator cannot manage Admin role, enforce that Admin membership cannot be newly assigned
            // and cannot be removed if the user already has it.
            if (!canManageAdminRole)
            {
                if (currentRoles.Contains("Admin") && !desiredRoles.Contains("Admin"))
                {
                    // keep Admin role for this user
                    desiredRoles.Add("Admin");
                }
                if (!currentRoles.Contains("Admin") && desiredRoles.Contains("Admin"))
                {
                    // remove attempted Admin assignment by non-protected user
                    desiredRoles.RemoveAll(r => r == "Admin");
                }
            }

            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (desiredRoles.Any())
            {
                await _userManager.AddToRolesAsync(user, desiredRoles);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
