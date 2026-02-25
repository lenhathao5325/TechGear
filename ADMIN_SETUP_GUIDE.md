# Hướng Dẫn Cấu Hình Admin Area

## ✅ Các Thay Đổi Đã Thực Hiện

### 1. **Tạo Controllers cho Admin Area**
Đã tạo các controllers sau trong `TechGear/Areas/Admin/Controllers/`:

- ✅ `DashboardController.cs` - Quản lý trang dashboard
- ✅ `AdminBrandsController.cs` - Quản lý thương hiệu (CRUD)
- ✅ `AdminCategoriesController.cs` - Quản lý danh mục (CRUD)
- ✅ `AdminProductsController.cs` - Quản lý sản phẩm (CRUD)

### 2. **Cập Nhật Services**
- ✅ `IApiService.cs` - Thêm các phương thức generic:
  - `GetAsync<T>(string endpoint)`
  - `PostAsync<T>(string endpoint, object data)`
  - `PutAsync<T>(string endpoint, object data)`
  - `DeleteAsync(string endpoint)`

- ✅ `ApiService.cs` - Implement các phương thức generic để call API

### 3. **Cập Nhật Models**
- ✅ `Product.cs` - Thêm thuộc tính `ProductId`, `ProductName`, `BasePrice`, `IsActive`
- ✅ `Category.cs` - Thêm thuộc tính `ImageUrl`
- ✅ `Brand.cs` - Đã có đầy đủ thuộc tính

### 4. **Cập Nhật Layout**
- ✅ `_AdminLayout.cshtml` - Cập nhật:
  - Sử dụng asp-tag helpers cho tất cả links
  - Thêm hiển thị TempData messages (Success/Error)
  - Thêm link "Về trang chủ"

- ✅ `_ViewImports.cshtml` - Thêm namespace `TechGear.Models.ViewModels`

### 5. **Cấu Hình API**
- ✅ `appsettings.json` - Sửa BaseUrl từ `https://localhost:3636/api` → `https://localhost:3636`

## 📋 Cấu Trúc API Endpoints

### TechGearAPI (Port 3636)
```
├── api/Brands
│   ├── GET    /api/Brands          - Lấy tất cả brands
│   ├── GET    /api/Brands/{id}     - Lấy brand theo ID
│   ├── POST   /api/Brands          - Tạo brand mới
│   ├── PUT    /api/Brands/{id}     - Cập nhật brand
│   └── DELETE /api/Brands/{id}     - Xóa brand
│
├── api/Categories
│   ├── GET    /api/Categories          - Lấy tất cả categories
│   ├── GET    /api/Categories/{id}     - Lấy category theo ID
│   ├── POST   /api/Categories          - Tạo category mới
│   ├── PUT    /api/Categories/{id}     - Cập nhật category
│   └── DELETE /api/Categories/{id}     - Xóa category
│
└── api/products
    ├── GET    /api/products              - Lấy tất cả products
    ├── GET    /api/products/{id}         - Lấy product theo ID
    ├── GET    /api/products/latest/{count} - Lấy products mới nhất
    ├── GET    /api/products/category/{categoryId} - Lấy products theo category
    ├── GET    /api/products/brand/{brandId} - Lấy products theo brand
    ├── POST   /api/products              - Tạo product mới
    ├── PUT    /api/products/{id}         - Cập nhật product
    └── DELETE /api/products/{id}         - Xóa product
```

## 🔗 Các Routes Admin

### MVC Routes (TechGear)
```
├── /Admin/Dashboard/Index              - Trang dashboard
├── /Admin/AdminBrands
│   ├── /Index                          - Danh sách brands
│   ├── /Create                         - Tạo brand
│   ├── /Edit/{id}                      - Sửa brand
│   ├── /Details/{id}                   - Chi tiết brand
│   └── /Delete/{id}                    - Xóa brand
│
├── /Admin/AdminCategories
│   ├── /Index                          - Danh sách categories
│   ├── /Create                         - Tạo category
│   ├── /Edit/{id}                      - Sửa category
│   ├── /Details/{id}                   - Chi tiết category
│   └── /Delete/{id}                    - Xóa category
│
└── /Admin/AdminProducts
    ├── /Index                          - Danh sách products
    ├── /Create                         - Tạo product
    ├── /Edit/{id}                      - Sửa product
    ├── /Details/{id}                   - Chi tiết product
    └── /Delete/{id}                    - Xóa product
```

## 🚀 Cách Chạy

### 1. Khởi động API
```bash
cd TearGearAPI
dotnet run
```
API sẽ chạy tại: `https://localhost:3636`

### 2. Khởi động MVC
```bash
cd TechGear
dotnet run
```
MVC sẽ chạy tại port được config trong launchSettings.json

### 3. Truy cập Admin Area
```
https://localhost:{mvc-port}/Admin/Dashboard
```

## 📝 DTO Format cho API

### Brand DTO
```json
{
  "Name": "Tên thương hiệu"
}
```

### Category DTO
```json
{
  "Name": "Tên danh mục"
}
```

### Product DTO
```json
{
  "Name": "Tên sản phẩm",
  "Description": "Mô tả",
  "CategoryId": 1,
  "BrandId": 1,
  "ImageUrl": "url-hình-ảnh"
}
```

## 🎨 Tính Năng Hiện Có

### Dashboard
- ✅ Hiển thị thống kê tổng quan
- ✅ Biểu đồ doanh thu (sử dụng Chart.js)
- ✅ Danh sách đơn hàng mới nhất

### Quản Lý Brands
- ✅ Xem danh sách brands
- ✅ Tạo brand mới
- ✅ Sửa brand
- ✅ Xóa brand
- ✅ Xem chi tiết brand

### Quản Lý Categories
- ✅ Xem danh sách categories
- ✅ Tạo category mới
- ✅ Sửa category
- ✅ Xóa category
- ✅ Xem chi tiết category

### Quản Lý Products
- ✅ Xem danh sách products
- ✅ Tạo product mới (với dropdown Category & Brand)
- ✅ Sửa product
- ✅ Xóa product
- ✅ Xem chi tiết product

## 🔧 Các File Quan Trọng

### MVC (TechGear)
```
TechGear/
├── Areas/Admin/
│   ├── Controllers/
│   │   ├── DashboardController.cs
│   │   ├── AdminBrandsController.cs
│   │   ├── AdminCategoriesController.cs
│   │   └── AdminProductsController.cs
│   └── Views/
│       ├── Dashboard/Index.cshtml
│       ├── AdminBrands/{Index,Create,Edit,Delete,Details}.cshtml
│       ├── AdminCategories/{Index,Create,Edit,Delete,Details}.cshtml
│       ├── AdminProducts/{Index,Create,Edit,Delete,Details}.cshtml
│       └── Shared/_AdminLayout.cshtml
├── Services/
│   ├── IApiService.cs
│   └── ApiService.cs
├── Models/
│   ├── Product.cs
│   ├── Category.cs
│   └── Brand.cs
├── Program.cs
└── appsettings.json
```

### API (TearGearAPI)
```
TearGearAPI/
├── Controllers/
│   ├── BrandsController.cs
│   ├── CategoriesController.cs
│   └── ProductAPIsController.cs
├── DTO/
│   ├── BrandDTO.cs
│   ├── CategoryDTO.cs
│   └── ProductDTO.cs
└── Program.cs
```

## ⚠️ Lưu Ý

1. **CORS**: API đã được cấu hình CORS với policy "AllowMVC" - cho phép tất cả origins
2. **Port**: Đảm bảo API chạy ở port 3636 như đã config
3. **Database**: Cần có database và chạy migrations trước khi sử dụng
4. **Authentication**: Hiện tại chưa có authentication cho Admin - cần thêm sau
5. **Views**: Các views (Index, Create, Edit, Delete, Details) đã có sẵn trong Areas/Admin/Views

## 🔜 Tính Năng Cần Thêm (Tùy Chọn)

- [ ] Authentication & Authorization cho Admin
- [ ] Upload hình ảnh với Cloudinary
- [ ] Quản lý đơn hàng (Orders)
- [ ] Quản lý users
- [ ] Phân quyền chi tiết (Roles & Permissions)
- [ ] Thống kê chi tiết hơn
- [ ] Export/Import data
- [ ] Audit logs

## 🐛 Troubleshooting

### Lỗi kết nối API
- Kiểm tra API đang chạy chưa
- Kiểm tra port trong appsettings.json
- Kiểm tra CORS settings

### Lỗi 404 Not Found
- Kiểm tra routing trong Program.cs
- Kiểm tra [Area("Admin")] attribute trong controllers

### Lỗi hiển thị Views
- Kiểm tra _ViewStart.cshtml có đúng layout không
- Kiểm tra namespace trong _ViewImports.cshtml

## 📞 Liên Hệ Support

Nếu có vấn đề, kiểm tra:
1. Cả 2 projects (API và MVC) đều đã build thành công
2. Database connection string đúng
3. Migrations đã chạy
4. CORS được enable ở API
