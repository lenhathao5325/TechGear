# 🔐 THÔNG TIN TÀI KHOẢN ADMIN - TECHGEAR

## 📋 Danh sách tài khoản đã được seed tự động

### 1. 👨‍💼 ADMIN (Quản trị viên)
```
📧 Email: admin@techgear.vn
🔑 Password: Admin@123456
👤 Họ tên: Administrator
📱 Số điện thoại: 0123456789
🎭 Role: Admin
✅ Quyền: Toàn quyền truy cập Dashboard, quản lý tất cả chức năng
```

**Quyền truy cập:**
- ✅ Dashboard
- ✅ Quản lý sản phẩm (Products)
- ✅ Quản lý danh mục (Categories)
- ✅ Quản lý thương hiệu (Brands)
- ✅ Quản lý đơn hàng (Orders) - **Chỉ Admin**
- ✅ Quản lý khách hàng (Customers) - **Chỉ Admin**

---

### 2. 👔 STAFF (Nhân viên)
```
📧 Email: staff@techgear.vn
🔑 Password: Staff@123456
👤 Họ tên: Staff User
📱 Số điện thoại: 0987654321
🎭 Role: Staff
⚠️ Quyền: Giới hạn, không truy cập được Orders và Customers
```

**Quyền truy cập:**
- ✅ Dashboard
- ✅ Quản lý sản phẩm (Products)
- ✅ Quản lý danh mục (Categories)
- ✅ Quản lý thương hiệu (Brands)
- ❌ Quản lý đơn hàng (Orders) - **Bị khóa**
- ❌ Quản lý khách hàng (Customers) - **Bị khóa**

---

### 3. 👤 USER (Khách hàng thử nghiệm)
```
📧 Email: user@techgear.vn
🔑 Password: User@123456
👤 Họ tên: Test User
📱 Số điện thoại: 0111222333
🎭 Role: User
🛒 Quyền: Chỉ mua hàng trên trang web
```

**Quyền truy cập:**
- ❌ Không thể truy cập Dashboard
- ✅ Mua hàng trên trang web
- ✅ Xem giỏ hàng
- ✅ Quản lý đơn hàng của mình
- ✅ Cập nhật thông tin cá nhân

---

## 🚀 HƯỚNG DẪN SỬ DỤNG

### Bước 1: Chạy API (Backend)
```bash
cd TearGearAPI
dotnet run
```
- API sẽ chạy trên: `https://localhost:7034`
- Swagger: `https://localhost:7034/swagger`

### Bước 2: Kiểm tra Seed
Khi API khởi động, bạn sẽ thấy log:
```
✅ Created Admin user: admin@techgear.vn
✅ Created Staff user: staff@techgear.vn
✅ Created Test user: user@techgear.vn
```

Nếu đã tồn tại:
```
ℹ️ Admin user already exists: admin@techgear.vn
ℹ️ Staff user already exists: staff@techgear.vn
ℹ️ Test user already exists: user@techgear.vn
```

### Bước 3: Chạy MVC (Frontend)
```bash
cd TechGear
dotnet run
```
- Web sẽ chạy trên: `https://localhost:7206`

### Bước 4: Đăng nhập vào Dashboard
1. Mở trình duyệt và truy cập: `https://localhost:7206`
2. Click **Đăng nhập**
3. Nhập thông tin:
   - Email: `admin@techgear.vn`
   - Password: `Admin@123456`
4. Sau khi đăng nhập thành công, truy cập: `https://localhost:7206/Admin`

---

## 🔧 TROUBLESHOOTING

### ❌ Lỗi: "User already exists"
**Nguyên nhân:** Tài khoản đã được seed trước đó.
**Giải pháp:** Bình thường, không cần làm gì cả.

### ❌ Lỗi: "Access Denied khi truy cập /Admin"
**Nguyên nhân:** 
- Đang đăng nhập với tài khoản `user@techgear.vn` (Role: User)
- Tài khoản User không có quyền truy cập Dashboard

**Giải pháp:**
1. Đăng xuất
2. Đăng nhập lại với `admin@techgear.vn` hoặc `staff@techgear.vn`

### ❌ Lỗi: "Cannot access Orders page"
**Nguyên nhân:** Đang đăng nhập với tài khoản Staff, chỉ Admin mới truy cập được.
**Giải pháp:** Đăng nhập với `admin@techgear.vn`

### ❌ Lỗi: "Không thể đăng nhập"
**Nguyên nhân:** 
- API chưa chạy
- Sai mật khẩu

**Giải pháp:**
1. Kiểm tra API đã chạy chưa (`https://localhost:7034/swagger`)
2. Kiểm tra lại email và password (phân biệt chữ hoa/thường)
3. Xóa database và chạy lại migration:
   ```bash
   cd TearGearAPI
   dotnet ef database drop
   dotnet ef database update
   dotnet run
   ```

---

## 🔄 RESET TÀI KHOẢN

### Cách 1: Xóa và tạo lại Database
```bash
cd TearGearAPI
dotnet ef database drop
dotnet ef database update
dotnet run
```

### Cách 2: Chạy SQL Script trực tiếp
```sql
-- Xóa users cũ
DELETE FROM AspNetUsers WHERE Email IN ('admin@techgear.vn', 'staff@techgear.vn', 'user@techgear.vn');

-- Khởi động lại API để seed lại
```

---

## 📝 GHI CHÚ QUAN TRỌNG

### Bảo mật
⚠️ **CẢNH BÁO:** Đây là tài khoản mặc định cho môi trường Development.
- ❌ **KHÔNG** sử dụng mật khẩu này trong Production
- ❌ **KHÔNG** commit file này lên Git public
- ✅ Thay đổi mật khẩu ngay sau khi deploy Production

### Phân quyền
- **Admin**: Toàn quyền
- **Staff**: Giới hạn (không có Orders và Customers)
- **User**: Chỉ mua hàng, không vào Dashboard

### Session
- Session timeout: 30 phút
- Sau 30 phút không hoạt động, bạn sẽ bị đăng xuất tự động

---

## 📞 HỖ TRỢ

Nếu gặp vấn đề, kiểm tra:
1. ✅ API đã chạy: `https://localhost:7034/swagger`
2. ✅ MVC đã chạy: `https://localhost:7206`
3. ✅ Database đã được migrate
4. ✅ Seed log hiển thị thành công

**Log seed thành công:**
```
✅ Seeded role: Admin
✅ Seeded role: Staff
✅ Seeded role: User
✅ Created Admin user: admin@techgear.vn
✅ Created Staff user: staff@techgear.vn
✅ Created Test user: user@techgear.vn
```

---

## 🎯 NHANH CHÓNG

### Đăng nhập nhanh Dashboard:
1. Start API: `cd TearGearAPI && dotnet run`
2. Start MVC: `cd TechGear && dotnet run`
3. Truy cập: `https://localhost:7206`
4. Login: `admin@techgear.vn` / `Admin@123456`
5. Vào Dashboard: `https://localhost:7206/Admin`

---

**Ngày tạo:** 25/02/2026  
**Phiên bản:** 1.0  
**Dự án:** TechGear E-Commerce System
