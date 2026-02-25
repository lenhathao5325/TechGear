# ✅ Checklist Kiểm Tra Admin Area

## 🎯 Các Bước Kiểm Tra

### 1. Kiểm Tra Cấu Hình
- [x] Controllers đã được tạo trong `TechGear/Areas/Admin/Controllers/`
- [x] Services đã có methods generic (GetAsync, PostAsync, PutAsync, DeleteAsync)
- [x] Models đã có đầy đủ properties
- [x] appsettings.json có BaseUrl đúng (https://localhost:3636)
- [x] Program.cs có route cho Areas
- [x] _AdminLayout.cshtml sử dụng asp-tag helpers
- [x] Build thành công (No errors)

### 2. Kiểm Tra Kết Nối API
```bash
# Chạy API
cd TearGearAPI
dotnet run

# Kiểm tra Swagger
# Mở browser: https://localhost:3636/swagger

# Test endpoints:
GET https://localhost:3636/api/Brands
GET https://localhost:3636/api/Categories
GET https://localhost:3636/api/products
```

### 3. Kiểm Tra MVC
```bash
# Chạy MVC
cd TechGear
dotnet run

# Truy cập các trang:
https://localhost:{port}/Admin/Dashboard
https://localhost:{port}/Admin/AdminBrands
https://localhost:{port}/Admin/AdminCategories
https://localhost:{port}/Admin/AdminProducts
```

### 4. Test Các Chức Năng

#### Dashboard
- [ ] Mở được trang Dashboard
- [ ] Hiển thị thống kê
- [ ] Biểu đồ Chart.js load được
- [ ] Sidebar menu hoạt động

#### Brands
- [ ] Xem danh sách brands
- [ ] Tạo brand mới
- [ ] Edit brand
- [ ] Delete brand
- [ ] Xem details brand
- [ ] Hiển thị thông báo Success/Error

#### Categories
- [ ] Xem danh sách categories
- [ ] Tạo category mới
- [ ] Edit category
- [ ] Delete category
- [ ] Xem details category
- [ ] Hiển thị thông báo Success/Error

#### Products
- [ ] Xem danh sách products
- [ ] Dropdown Category & Brand hoạt động
- [ ] Tạo product mới
- [ ] Edit product
- [ ] Delete product
- [ ] Xem details product
- [ ] Hiển thị thông báo Success/Error

## 🔍 Debug Common Issues

### API không kết nối được
```bash
# Kiểm tra API có chạy không
netstat -ano | findstr :3636

# Kiểm tra CORS
# Trong API Program.cs phải có:
app.UseCors("AllowMVC");
```

### Views không hiển thị
```csharp
// Kiểm tra _ViewStart.cshtml
@{
    Layout = "~/Areas/Admin/Views/Shared/_AdminLayout.cshtml";
}

// Kiểm tra Controller có [Area("Admin")]
[Area("Admin")]
public class DashboardController : Controller
```

### TempData không hiển thị
```csharp
// Trong Program.cs phải có:
app.UseSession();

// Trong _AdminLayout.cshtml phải có:
@if (TempData["Success"] != null) { ... }
@if (TempData["Error"] != null) { ... }
```

## 📊 Expected Results

### Brands API Response
```json
[
  {
    "BrandId": 1,
    "BrandName": "Apple",
    "IsActive": true
  }
]
```

### Categories API Response
```json
[
  {
    "CategoryId": 1,
    "CategoryName": "Laptop",
    "IsActive": true
  }
]
```

### Products API Response
```json
[
  {
    "Id": 1,
    "Name": "MacBook Pro",
    "Description": "...",
    "ImageUrl": "...",
    "CategoryId": 1,
    "BrandId": 1,
    "Category": "Laptop",
    "Brand": "Apple"
  }
]
```

## 🚦 Status

- ✅ **Controllers**: Created & Working
- ✅ **Services**: Updated with Generic Methods
- ✅ **Models**: Updated with Required Properties
- ✅ **API Endpoints**: Configured Correctly
- ✅ **Routing**: Areas Route Configured
- ✅ **Layout**: Updated with Tag Helpers
- ✅ **Build**: Successful
- ⏳ **Runtime Test**: Pending (Cần chạy cả 2 projects)

## 📝 Next Steps

1. ✅ Start API Project (TearGearAPI)
2. ✅ Start MVC Project (TechGear)
3. ⏳ Test Dashboard: `https://localhost:{port}/Admin/Dashboard`
4. ⏳ Test CRUD operations for Brands
5. ⏳ Test CRUD operations for Categories
6. ⏳ Test CRUD operations for Products

## 🎉 Completion Criteria

Tất cả checkbox trên được check ✅ = Admin Area hoạt động hoàn hảo!
