-- Script SQL để tạo Admin và Staff users
-- Chạy sau khi đã run migration và seed roles

-- Lưu ý: Password phải được hash bằng Identity
-- Không thể insert trực tiếp hash password vào database

-- Để tạo Admin và Staff, bạn có 2 cách:

-- CÁCH 1: Sử dụng Package Manager Console trong Visual Studio
-- ----------------------------------------------------------------
-- Mở Package Manager Console
-- Chọn Default project: TechGearAPI
-- Chạy lệnh sau:

/*
Add-Migration AddAdminAndStaffUsers
Update-Database
*/

-- Sau đó thêm code vào Migration file:

/*
using Microsoft.AspNetCore.Identity;
using TechGearAPI.Models;

protected override void Up(MigrationBuilder migrationBuilder)
{
    // This will be handled by ApplicationDbContextSeed
}
*/

-- CÁCH 2: Tạo ApplicationDbContextSeed.cs (KHUYẾN NGHỊ)
-- ----------------------------------------------------------------
-- Tạo file: TearGearAPI/Data/ApplicationDbContextSeed.cs
-- Nội dung:

/*
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
            }
        }
    }
}
*/

-- Sau đó cập nhật Program.cs:

/*
// Seed Roles
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    
    // Seed roles
    await RoleSeeder.SeedRoles(services);
    
    // Seed admin and staff users
    await ApplicationDbContextSeed.SeedAdminAndStaff(services);
}
*/

-- ================================================================
-- THÔNG TIN ĐĂNG NHẬP SAU KHI SEED
-- ================================================================

/*
ADMIN ACCOUNT:
Email: admin@techgear.vn
Password: Admin@123456
Role: Admin
Quyền: Toàn bộ quản lý (sản phẩm, đơn hàng, khách hàng)

STAFF ACCOUNT:
Email: staff@techgear.vn
Password: Staff@123456
Role: Staff
Quyền: Quản lý sản phẩm (KHÔNG được quản lý đơn hàng)

TEST USER ACCOUNT:
Email: user@techgear.vn
Password: User@123456
Role: User
Quyền: Mua hàng, xem đơn hàng của mình
*/

-- ================================================================
-- LƯU Ý BẢO MẬT
-- ================================================================

/*
1. ĐỔI PASSWORD NGAY SAU KHI DEPLOY PRODUCTION!

2. Xóa hoặc comment các dòng seed user trong production

3. Sử dụng environment variables cho sensitive data

4. Enable 2FA cho admin accounts

5. Password requirements:
   - Ít nhất 8 ký tự
   - Có chữ hoa (A-Z)
   - Có chữ thường (a-z)
   - Có số (0-9)
   - Có ký tự đặc biệt (@, #, $, ...)
*/
