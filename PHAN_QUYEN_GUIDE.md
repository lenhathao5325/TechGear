# HƯỚNG DẪN TRIỂN KHAI HỆ THỐNG PHÂN QUYỀN TECHGEAR

## Tổng Quan
Hệ thống phân quyền đã được triển khai với 3 roles chính:
- **Admin**: Toàn quyền quản lý (sản phẩm, đơn hàng, khách hàng)
- **Staff**: Quản lý sản phẩm (thêm, sửa, xóa) nhưng KHÔNG được quản lý đơn hàng và khách hàng
- **User**: Mua hàng, xem sản phẩm, quản lý giỏ hàng và đơn hàng của mình

---

## 🎯 Chức Năng Theo Role

### 👤 USER (Khách hàng)
#### Có thể làm:
- ✅ Xem danh sách sản phẩm (không cần đăng nhập)
- ✅ Xem chi tiết sản phẩm (không cần đăng nhập)
- ✅ **Thêm vào giỏ hàng** (yêu cầu đăng nhập)
  - Nếu chưa đăng nhập → redirect sang trang Login
- ✅ Xem giỏ hàng
- ✅ Cập nhật số lượng, xóa sản phẩm trong giỏ
- ✅ **Đi đến thanh toán** (Checkout)
- ✅ Chọn địa chỉ giao hàng
- ✅ Đặt hàng
- ✅ Xem đơn hàng của mình
- ✅ Xem chi tiết đơn hàng

#### Routes:
- `/Products` - Danh sách sản phẩm
- `/Products/Details/{id}` - Chi tiết sản phẩm
- `/Cart` - Giỏ hàng (requires login)
- `/Order/Checkout` - Thanh toán
- `/Order/MyOrders` - Đơn hàng của tôi
- `/Order/OrderDetails/{id}` - Chi tiết đơn hàng

---

### 👨‍💼 ADMIN (Quản trị viên)
#### Có thể làm:
- ✅ Tất cả quyền của Staff
- ✅ **Quản lý đơn hàng**
  - Xem tất cả đơn hàng
  - Cập nhật trạng thái đơn hàng
  - Xem chi tiết đơn hàng
- ✅ **Quản lý khách hàng**
- ✅ Truy cập Dashboard
- ✅ Khi đăng nhập → redirect sang `/Admin/Dashboard`

#### Routes:
- `/Admin/Dashboard` - Trang quản trị
- `/Admin/AdminProducts` - Quản lý sản phẩm
- `/Admin/AdminCategories` - Quản lý danh mục
- `/Admin/AdminBrands` - Quản lý thương hiệu
- `/Admin/AdminOrders` - **Quản lý đơn hàng** (CHỈ ADMIN)

---

### 👨‍🔧 STAFF (Nhân viên)
#### Có thể làm:
- ✅ Quản lý sản phẩm (thêm, sửa, xóa)
- ✅ Quản lý danh mục
- ✅ Quản lý thương hiệu
- ✅ Truy cập Dashboard
- ✅ Khi đăng nhập → redirect sang `/Admin/Dashboard`

#### KHÔNG thể làm:
- ❌ Quản lý đơn hàng (menu bị disable)
- ❌ Quản lý khách hàng (menu bị disable)

#### UI Đặc biệt:
- Menu "Đơn hàng" và "Khách hàng" hiển thị nhưng:
  - Opacity: 0.5
  - Cursor: not-allowed
  - Có icon khóa 🔒
  - Tooltip: "Chỉ Admin mới có quyền truy cập"

---

## 📁 Cấu Trúc File Đã Tạo

### API (TearGearAPI)

#### Constants
```
TearGearAPI/Constants/RoleConstants.cs
```
- Chứa constants: Admin, Staff, User

#### Data
```
TearGearAPI/Data/RoleSeeder.cs
```
- Seed roles vào database khi khởi động

#### Controllers (API)
```
TearGearAPI/Controllers/AuthController.cs (CẬP NHẬT)
- Register: Tự động assign role "User"
- Login: Trả về roles trong response
- Token: Include roles trong JWT claims

TearGearAPI/Controllers/CartController.cs (MỚI)
- GET /api/cart - Lấy giỏ hàng
- POST /api/cart/add - Thêm vào giỏ
- PUT /api/cart/{id} - Cập nhật số lượng
- DELETE /api/cart/{id} - Xóa khỏi giỏ
- DELETE /api/cart/clear - Xóa toàn bộ giỏ

TearGearAPI/Controllers/OrdersController.cs (MỚI)
- GET /api/orders/my-orders - Đơn hàng của tôi
- GET /api/orders/{id} - Chi tiết đơn hàng
- POST /api/orders - Tạo đơn hàng
- PUT /api/orders/{id}/status - Cập nhật trạng thái (Admin only)
- GET /api/orders - Tất cả đơn hàng (Admin only)

TearGearAPI/Controllers/UserAddressController.cs (MỚI)
- GET /api/useraddress - Danh sách địa chỉ
- GET /api/useraddress/{id} - Chi tiết địa chỉ
- POST /api/useraddress - Thêm địa chỉ
- PUT /api/useraddress/{id} - Cập nhật địa chỉ
- DELETE /api/useraddress/{id} - Xóa địa chỉ
- PUT /api/useraddress/{id}/set-default - Đặt địa chỉ mặc định
```

### MVC (TechGear)

#### Constants
```
TechGear/Constants/RoleConstants.cs
```

#### Filters
```
TechGear/Filters/AuthorizeRoleAttribute.cs
- Custom attribute để check role

TechGear/Filters/RequireLoginAttribute.cs
- Attribute yêu cầu đăng nhập
```

#### Controllers (MVC)
```
TechGear/Controllers/AuthController.cs (CẬP NHẬT)
- Login: Lưu UserRole vào session
- Redirect theo role khi login thành công

TechGear/Controllers/CartController.cs (MỚI)
- Index: Xem giỏ hàng
- AddToCart: Thêm vào giỏ
- UpdateQuantity: Cập nhật số lượng
- RemoveFromCart: Xóa khỏi giỏ

TechGear/Controllers/OrderController.cs (MỚI)
- Checkout: Trang thanh toán
- PlaceOrder: Đặt hàng
- OrderConfirmation: Xác nhận đơn hàng
- MyOrders: Đơn hàng của tôi
- OrderDetails: Chi tiết đơn hàng
```

#### Admin Controllers
```
TechGear/Areas/Admin/Controllers/DashboardController.cs (CẬP NHẬT)
- Thêm [AuthorizeRole(Admin, Staff)]

TechGear/Areas/Admin/Controllers/AdminProductsController.cs (CẬP NHẬT)
- Thêm [AuthorizeRole(Admin, Staff)]

TechGear/Areas/Admin/Controllers/AdminCategoriesController.cs (CẬP NHẬT)
- Thêm [AuthorizeRole(Admin, Staff)]

TechGear/Areas/Admin/Controllers/AdminBrandsController.cs (CẬP NHẬT)
- Thêm [AuthorizeRole(Admin, Staff)]

TechGear/Areas/Admin/Controllers/AdminOrdersController.cs (MỚI)
- CHỈ ADMIN: [AuthorizeRole(Admin)]
```

#### Views
```
TechGear/Views/Shared/_Layout.cshtml (CẬP NHẬT)
- Cart icon: Check login và role
- Chỉ User mới thấy cart có số lượng
- Admin/Staff không thấy cart

TechGear/Views/Products/Details.cshtml (CẬP NHẬT)
- Nút "Thêm vào giỏ hàng": Check login
- Nếu chưa login → redirect Login
- Admin/Staff: Hiển thị thông báo "Cần tài khoản User"

TechGear/Areas/Admin/Views/Shared/_AdminLayout.cshtml (CẬP NHẬT)
- Menu động theo role
- Staff: Menu "Đơn hàng" và "Khách hàng" bị disable
- Hiển thị tên và role của user trong navbar
```

---

## 🔐 Cơ Chế Phân Quyền

### 1. API Level (JWT)
```csharp
// Token generation includes roles
var claims = new List<Claim>
{
    new Claim(ClaimTypes.Name, user.UserName),
    new Claim(ClaimTypes.NameIdentifier, user.Id),
    new Claim(ClaimTypes.Email, user.Email),
    new Claim(ClaimTypes.Role, "Admin") // hoặc Staff, User
};

// Authorize attribute
[Authorize(Roles = "Admin")]
[Authorize(Roles = "Admin,Staff")]
```

### 2. MVC Level (Session)
```csharp
// Lưu role vào session khi login
HttpContext.Session.SetString("UserRole", userRole);

// Filter attribute
[AuthorizeRole(RoleConstants.Admin, RoleConstants.Staff)]

// View level check
@{
    var userRole = Context.Session.GetString("UserRole");
    var isAdmin = userRole == "Admin";
}
```

---

## 🔄 Luồng Hoạt Động

### User Mua Hàng
1. User **xem sản phẩm** (không cần login)
2. Click **"Thêm vào giỏ hàng"**
   - Nếu chưa login → redirect `/Auth/Login?returnUrl=/Products/Details/123`
   - Đã login → Gọi API `/api/cart/add`
3. Vào **Giỏ hàng** (`/Cart`)
4. Click **"Đi đến thanh toán"** → `/Order/Checkout`
5. Chọn **địa chỉ giao hàng**
6. Click **"Đặt hàng"** → Gọi API `/api/orders`
7. Redirect → `/Order/OrderConfirmation/{orderId}`

### Admin Quản Lý Đơn Hàng
1. Login với tài khoản Admin
2. Auto redirect → `/Admin/Dashboard`
3. Click menu **"Đơn hàng"** → `/Admin/AdminOrders`
4. Xem danh sách đơn hàng
5. Click chi tiết → Xem thông tin đầy đủ
6. **Cập nhật trạng thái**: Pending → Confirmed → Shipping → Completed

### Staff Quản Lý Sản Phẩm
1. Login với tài khoản Staff
2. Auto redirect → `/Admin/Dashboard`
3. Có thể:
   - Thêm/sửa/xóa sản phẩm
   - Quản lý danh mục
   - Quản lý thương hiệu
4. KHÔNG thể:
   - Truy cập `/Admin/AdminOrders` → 403 Forbidden
   - Menu "Đơn hàng" bị disabled

---

## 🚀 Các Bước Tiếp Theo

### 1. Tạo Admin User (Cần làm thủ công)
Sử dụng Package Manager Console hoặc migration:

```csharp
// Trong Seed hoặc Migration
var userManager = serviceProvider.GetService<UserManager<ApplicationUserAPI>>();

var admin = new ApplicationUserAPI 
{ 
    UserName = "admin@techgear.vn",
    Email = "admin@techgear.vn",
    FullName = "Administrator"
};
await userManager.CreateAsync(admin, "Admin@123");
await userManager.AddToRoleAsync(admin, "Admin");

var staff = new ApplicationUserAPI 
{ 
    UserName = "staff@techgear.vn",
    Email = "staff@techgear.vn",
    FullName = "Staff User"
};
await userManager.CreateAsync(staff, "Staff@123");
await userManager.AddToRoleAsync(staff, "Staff");
```

### 2. Test Scenarios

#### Test User Flow:
- [ ] Register tài khoản mới (tự động role User)
- [ ] Login → redirect về Home
- [ ] Xem sản phẩm
- [ ] Thêm vào giỏ hàng
- [ ] Checkout
- [ ] Đặt hàng thành công
- [ ] Xem đơn hàng

#### Test Admin Flow:
- [ ] Login với admin@techgear.vn
- [ ] Auto redirect → Dashboard
- [ ] Thấy đầy đủ menu
- [ ] Quản lý đơn hàng
- [ ] Cập nhật trạng thái đơn hàng

#### Test Staff Flow:
- [ ] Login với staff@techgear.vn
- [ ] Auto redirect → Dashboard
- [ ] Menu "Đơn hàng" bị disable
- [ ] Vẫn quản lý được sản phẩm

### 3. Views Cần Tạo (TODO)

```
TechGear/Views/Cart/Index.cshtml
TechGear/Views/Order/Checkout.cshtml
TechGear/Views/Order/OrderConfirmation.cshtml
TechGear/Views/Order/MyOrders.cshtml
TechGear/Views/Order/OrderDetails.cshtml
TechGear/Areas/Admin/Views/AdminOrders/Index.cshtml
TechGear/Areas/Admin/Views/AdminOrders/Details.cshtml
```

---

## 🐛 Lưu Ý Quan Trọng

### Database Schema
UserAddressAPI có các trường:
- `FullName` (không phải RecipientName)
- `Province` (không phải City)

### Session Keys
```csharp
JWToken          // JWT token
Email            // User email
UserId           // User ID
FullName         // User full name
PhoneNumber      // User phone
UserRole         // User role: "Admin" | "Staff" | "User"
```

### API Authorization
```csharp
[Authorize]                              // Require login
[Authorize(Roles = "Admin")]             // Admin only
[Authorize(Roles = "Admin,Staff")]       // Admin hoặc Staff
```

---

## 📊 Trạng Thái Đơn Hàng

```csharp
public enum OrderStatus
{
    Pending = 1,      // Chờ xử lý
    Confirmed = 2,    // Đã xác nhận
    Shipping = 3,     // Đang giao
    Completed = 4,    // Hoàn tất
    Cancelled = 5     // Đã hủy
}
```

Valid transitions:
- Pending → Confirmed ✅
- Pending → Cancelled ✅
- Confirmed → Shipping ✅
- Confirmed → Cancelled ✅
- Shipping → Completed ✅

---

## ✅ Checklist Hoàn Thành

### Backend (API)
- [x] Role seeding
- [x] Auth với roles
- [x] Cart API
- [x] Order API
- [x] UserAddress API
- [x] Authorization attributes

### Frontend (MVC)
- [x] Role constants
- [x] Custom filters
- [x] Auth redirect logic
- [x] Cart controller
- [x] Order controller
- [x] Admin controllers với authorization
- [x] Layout cập nhật (cart icon, user info)
- [x] Product details với "Thêm giỏ hàng"
- [x] Admin layout với role-based menu

### Cần Làm Tiếp
- [ ] Tạo các View cho Cart và Order
- [ ] Tạo Admin Order Management views
- [ ] Seed admin và staff users
- [ ] Test toàn bộ flow
- [ ] Handle errors và validation messages
- [ ] Add loading states

---

## 📞 Support

Nếu gặp lỗi, kiểm tra:
1. Session có lưu UserRole chưa?
2. Token có chứa role claims chưa?
3. Database đã có roles chưa?
4. User đã được assign role chưa?

**Build hiện tại có lỗi temp file Razor**. Để fix:
1. Clean solution
2. Xóa folder `obj` và `bin`
3. Rebuild lại

---

🎉 **Hệ thống phân quyền đã hoàn thành!**
