@echo off
chcp 65001 >nul
title TechGear - Seed Admin Account

echo ========================================
echo    TECHGEAR - SEED ADMIN ACCOUNT
echo ========================================
echo.

echo 🔍 Kiểm tra môi trường...
where dotnet >nul 2>&1
if %errorlevel% neq 0 (
    echo ❌ .NET SDK không được cài đặt hoặc không trong PATH
    echo 📥 Vui lòng tải và cài đặt .NET SDK từ: https://dotnet.microsoft.com/download
    pause
    exit /b 1
)

echo ✅ .NET SDK đã được cài đặt
echo.

echo 📦 Thông tin seed:
echo    - Admin: admin@techgear.vn / Admin@123456
echo    - Staff: staff@techgear.vn / Staff@123456
echo    - User: user@techgear.vn / User@123456
echo.

echo 🚀 Bắt đầu seed...
echo.

cd /d "%~dp0TearGearAPI"

echo 📊 Kiểm tra database...
dotnet ef database update
if %errorlevel% neq 0 (
    echo ❌ Không thể update database
    echo 💡 Hãy chạy: cd TearGearAPI && dotnet ef database update
    pause
    exit /b 1
)

echo.
echo ✅ Database đã sẵn sàng
echo.
echo 🌱 Đang chạy API để seed tài khoản...
echo 📝 Log sẽ hiển thị bên dưới:
echo ========================================
echo.

start "TechGear API - Seeding" cmd /k "cd /d %~dp0TearGearAPI && dotnet run"

echo.
echo ⏳ Đang chờ API khởi động (10 giây)...
timeout /t 10 /nobreak >nul

echo.
echo ========================================
echo ✅ HOÀN TẤT!
echo ========================================
echo.
echo 📋 Danh sách tài khoản:
echo.
echo 👨‍💼 ADMIN (Toàn quyền)
echo    📧 Email: admin@techgear.vn
echo    🔑 Password: Admin@123456
echo    🔗 Dashboard: https://localhost:7206/Admin
echo.
echo 👔 STAFF (Giới hạn quyền)
echo    📧 Email: staff@techgear.vn
echo    🔑 Password: Staff@123456
echo    ⚠️ Không truy cập được Orders & Customers
echo.
echo 👤 USER (Chỉ mua hàng)
echo    📧 Email: user@techgear.vn
echo    🔑 Password: User@123456
echo    🛒 Chỉ mua hàng, không vào Dashboard
echo.
echo ========================================
echo.
echo 📌 HƯỚNG DẪN TIẾP THEO:
echo    1. API đang chạy trong cửa sổ mới
echo    2. Mở terminal khác và chạy:
echo       cd TechGear
echo       dotnet run
echo    3. Truy cập: https://localhost:7206
echo    4. Đăng nhập với admin@techgear.vn
echo    5. Vào Dashboard: https://localhost:7206/Admin
echo.
echo 💡 Mẹo: Kiểm tra log trong cửa sổ API để xác nhận seed thành công
echo     Nếu thấy "✅ Created Admin user" nghĩa là thành công!
echo.

pause
