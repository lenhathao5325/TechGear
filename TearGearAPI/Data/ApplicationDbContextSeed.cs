using Microsoft.AspNetCore.Identity;
using TechGearAPI.Constants;
using TechGearAPI.Models;

namespace TechGearAPI.Data
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedAdminAndStaff(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUserAPI>>();

            // Create Admin User
            var adminEmail = "admin@techgear.vn";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new ApplicationUserAPI
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FullName = "Administrator",
                    PhoneNumber = "0123456789",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123456");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, RoleConstants.Admin);
                    Console.WriteLine($"✅ Created Admin user: {adminEmail}");
                }
                else
                {
                    Console.WriteLine($"❌ Failed to create Admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                Console.WriteLine($"ℹ️ Admin user already exists: {adminEmail}");
            }

            // Create Staff User
            var staffEmail = "staff@techgear.vn";
            var staffUser = await userManager.FindByEmailAsync(staffEmail);

            if (staffUser == null)
            {
                staffUser = new ApplicationUserAPI
                {
                    UserName = staffEmail,
                    Email = staffEmail,
                    FullName = "Staff User",
                    PhoneNumber = "0987654321",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(staffUser, "Staff@123456");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(staffUser, RoleConstants.Staff);
                    Console.WriteLine($"✅ Created Staff user: {staffEmail}");
                }
                else
                {
                    Console.WriteLine($"❌ Failed to create Staff user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                Console.WriteLine($"ℹ️ Staff user already exists: {staffEmail}");
            }

            // Create Test User
            var userEmail = "user@techgear.vn";
            var testUser = await userManager.FindByEmailAsync(userEmail);

            if (testUser == null)
            {
                testUser = new ApplicationUserAPI
                {
                    UserName = userEmail,
                    Email = userEmail,
                    FullName = "Test User",
                    PhoneNumber = "0111222333",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(testUser, "User@123456");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(testUser, RoleConstants.User);
                    Console.WriteLine($"✅ Created Test user: {userEmail}");
                }
                else
                {
                    Console.WriteLine($"❌ Failed to create Test user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                Console.WriteLine($"ℹ️ Test user already exists: {userEmail}");
            }
        }
    }
}
