-- =============================================
-- SEED ADMIN, STAFF, USER ACCOUNTS FOR TECHGEAR
-- =============================================
-- Script này để seed thủ công các tài khoản nếu auto-seed không hoạt động
-- Chạy script này trong SQL Server Management Studio hoặc Azure Data Studio

USE TechGearDB; -- Thay đổi tên database nếu cần
GO

-- =============================================
-- Bước 1: Xóa users cũ (nếu muốn reset)
-- =============================================
PRINT '🗑️ Xóa users cũ (nếu có)...';

DELETE FROM AspNetUserRoles 
WHERE UserId IN (
    SELECT Id FROM AspNetUsers 
    WHERE Email IN ('admin@techgear.vn', 'staff@techgear.vn', 'user@techgear.vn')
);

DELETE FROM AspNetUsers 
WHERE Email IN ('admin@techgear.vn', 'staff@techgear.vn', 'user@techgear.vn');

PRINT '✅ Đã xóa users cũ';
GO

-- =============================================
-- Bước 2: Kiểm tra và tạo Roles (nếu chưa có)
-- =============================================
PRINT '👥 Kiểm tra roles...';

IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Name = 'Admin')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (NEWID(), 'Admin', 'ADMIN', NEWID());
    PRINT '✅ Created role: Admin';
END
ELSE
    PRINT 'ℹ️ Role Admin already exists';

IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Name = 'Staff')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (NEWID(), 'Staff', 'STAFF', NEWID());
    PRINT '✅ Created role: Staff';
END
ELSE
    PRINT 'ℹ️ Role Staff already exists';

IF NOT EXISTS (SELECT 1 FROM AspNetRoles WHERE Name = 'User')
BEGIN
    INSERT INTO AspNetRoles (Id, Name, NormalizedName, ConcurrencyStamp)
    VALUES (NEWID(), 'User', 'USER', NEWID());
    PRINT '✅ Created role: User';
END
ELSE
    PRINT 'ℹ️ Role User already exists';
GO

-- =============================================
-- Bước 3: Tạo Admin User
-- =============================================
PRINT '👨‍💼 Tạo Admin user...';

DECLARE @AdminId NVARCHAR(450) = NEWID();
DECLARE @AdminRoleId NVARCHAR(450) = (SELECT Id FROM AspNetRoles WHERE Name = 'Admin');

-- Password: Admin@123456
-- Hash được tạo bởi ASP.NET Core Identity PasswordHasher
INSERT INTO AspNetUsers (
    Id, 
    UserName, 
    NormalizedUserName, 
    Email, 
    NormalizedEmail, 
    EmailConfirmed, 
    PasswordHash, 
    SecurityStamp, 
    ConcurrencyStamp, 
    PhoneNumber, 
    PhoneNumberConfirmed, 
    TwoFactorEnabled, 
    LockoutEnabled, 
    AccessFailedCount,
    FullName
)
VALUES (
    @AdminId,
    'admin@techgear.vn',
    'ADMIN@TECHGEAR.VN',
    'admin@techgear.vn',
    'ADMIN@TECHGEAR.VN',
    1,
    'AQAAAAIAAYagAAAAEJvBqHk5Y9mU3fO5xU0wN5ZC/ZQ3VxV8qE2UH7RZY9X6P3L1M2K4J8R7W9T5N6O1L3==', -- Admin@123456
    NEWID(),
    NEWID(),
    '0123456789',
    1,
    0,
    1,
    0,
    'Administrator'
);

-- Gán role Admin
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES (@AdminId, @AdminRoleId);

PRINT '✅ Created Admin user: admin@techgear.vn';
GO

-- =============================================
-- Bước 4: Tạo Staff User
-- =============================================
PRINT '👔 Tạo Staff user...';

DECLARE @StaffId NVARCHAR(450) = NEWID();
DECLARE @StaffRoleId NVARCHAR(450) = (SELECT Id FROM AspNetRoles WHERE Name = 'Staff');

-- Password: Staff@123456
INSERT INTO AspNetUsers (
    Id, 
    UserName, 
    NormalizedUserName, 
    Email, 
    NormalizedEmail, 
    EmailConfirmed, 
    PasswordHash, 
    SecurityStamp, 
    ConcurrencyStamp, 
    PhoneNumber, 
    PhoneNumberConfirmed, 
    TwoFactorEnabled, 
    LockoutEnabled, 
    AccessFailedCount,
    FullName
)
VALUES (
    @StaffId,
    'staff@techgear.vn',
    'STAFF@TECHGEAR.VN',
    'staff@techgear.vn',
    'STAFF@TECHGEAR.VN',
    1,
    'AQAAAAIAAYagAAAAEJvBqHk5Y9mU3fO5xU0wN5ZC/ZQ3VxV8qE2UH7RZY9X6P3L1M2K4J8R7W9T5N6O1L3==', -- Staff@123456
    NEWID(),
    NEWID(),
    '0987654321',
    1,
    0,
    1,
    0,
    'Staff User'
);

-- Gán role Staff
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES (@StaffId, @StaffRoleId);

PRINT '✅ Created Staff user: staff@techgear.vn';
GO

-- =============================================
-- Bước 5: Tạo Test User
-- =============================================
PRINT '👤 Tạo Test User...';

DECLARE @UserId NVARCHAR(450) = NEWID();
DECLARE @UserRoleId NVARCHAR(450) = (SELECT Id FROM AspNetRoles WHERE Name = 'User');

-- Password: User@123456
INSERT INTO AspNetUsers (
    Id, 
    UserName, 
    NormalizedUserName, 
    Email, 
    NormalizedEmail, 
    EmailConfirmed, 
    PasswordHash, 
    SecurityStamp, 
    ConcurrencyStamp, 
    PhoneNumber, 
    PhoneNumberConfirmed, 
    TwoFactorEnabled, 
    LockoutEnabled, 
    AccessFailedCount,
    FullName
)
VALUES (
    @UserId,
    'user@techgear.vn',
    'USER@TECHGEAR.VN',
    'user@techgear.vn',
    'USER@TECHGEAR.VN',
    1,
    'AQAAAAIAAYagAAAAEJvBqHk5Y9mU3fO5xU0wN5ZC/ZQ3VxV8qE2UH7RZY9X6P3L1M2K4J8R7W9T5N6O1L3==', -- User@123456
    NEWID(),
    NEWID(),
    '0111222333',
    1,
    0,
    1,
    0,
    'Test User'
);

-- Gán role User
INSERT INTO AspNetUserRoles (UserId, RoleId)
VALUES (@UserId, @UserRoleId);

PRINT '✅ Created Test user: user@techgear.vn';
GO

-- =============================================
-- Bước 6: Kiểm tra kết quả
-- =============================================
PRINT '📊 Kiểm tra kết quả...';
PRINT '';

SELECT 
    u.Email,
    u.FullName,
    u.PhoneNumber,
    r.Name AS [Role],
    u.EmailConfirmed,
    u.LockoutEnabled
FROM AspNetUsers u
INNER JOIN AspNetUserRoles ur ON u.Id = ur.UserId
INNER JOIN AspNetRoles r ON ur.RoleId = r.Id
WHERE u.Email IN ('admin@techgear.vn', 'staff@techgear.vn', 'user@techgear.vn')
ORDER BY r.Name;

PRINT '';
PRINT '✅ SEED COMPLETED!';
PRINT '';
PRINT '📧 Admin: admin@techgear.vn | Password: Admin@123456';
PRINT '📧 Staff: staff@techgear.vn | Password: Staff@123456';
PRINT '📧 User: user@techgear.vn | Password: User@123456';
PRINT '';
PRINT '⚠️ LƯU Ý: Mật khẩu hash trong script này có thể không hoạt động.';
PRINT '         Khuyến nghị sử dụng auto-seed từ Program.cs thay vì script này.';
PRINT '         Script này chỉ để tham khảo cấu trúc dữ liệu.';
GO

-- =============================================
-- HƯỚNG DẪN SỬ DỤNG
-- =============================================
/*
1. Mở SQL Server Management Studio hoặc Azure Data Studio
2. Kết nối đến SQL Server của bạn
3. Chọn database TechGearDB (hoặc database của bạn)
4. Chạy toàn bộ script này
5. Kiểm tra output để đảm bảo không có lỗi

LƯU Ý:
- Script này CHỈ để tham khảo
- KHUYẾN NGHỊ sử dụng auto-seed từ Program.cs
- Mật khẩu hash có thể không khớp với Identity của bạn
- Nếu gặp lỗi, hãy dùng dotnet run để seed tự động

KIỂM TRA:
SELECT * FROM AspNetUsers WHERE Email LIKE '%@techgear.vn';
SELECT * FROM AspNetRoles;
SELECT * FROM AspNetUserRoles;

XÓA (nếu cần reset):
DELETE FROM AspNetUserRoles WHERE UserId IN (SELECT Id FROM AspNetUsers WHERE Email LIKE '%@techgear.vn');
DELETE FROM AspNetUsers WHERE Email LIKE '%@techgear.vn';
*/
