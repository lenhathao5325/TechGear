# 🐛 KHẮC PHỤC LỖI HIỂN THỊ "N/A" TRONG ADMIN PANEL

## ❌ Vấn đề
Khi hiển thị danh sách sản phẩm trong Admin Panel, các cột **Danh mục** và **Thương hiệu** đều hiển thị "N/A" thay vì tên thật, mặc dù đã nhập đầy đủ thông tin.

### Screenshot lỗi:
```
Name              | Danh mục | Thương hiệu | Description | Hình ảnh
Laptop MSI Thin 15| N/A      | N/A         | mạnh mẽ     | [Image]
```

---

## 🔍 Nguyên nhân

### 1. **API Response không khớp với Model**

**API trả về:**
```csharp
Select(p => new
{
    // ...
    Category = p.Category.CategoryName,  // ❌ Tên property: "Category"
    Brand = p.Brand.BrandName            // ❌ Tên property: "Brand"
})
```

**MVC Model (Product.cs) expect:**
```csharp
public class Product
{
    // ...
    public string? CategoryName { get; set; }  // ✅ Expect "CategoryName"
    public string? BrandName { get; set; }     // ✅ Expect "BrandName"
}
```

**View hiển thị:**
```razor
<td>
    <span class="badge bg-info">
        @(item.CategoryName ?? "N/A")  // ✅ Dùng CategoryName
    </span>
</td>
<td>
    <span class="badge bg-warning">
        @(item.BrandName ?? "N/A")     // ✅ Dùng BrandName
    </span>
</td>
```

### 2. **Vấn đề Deserialization**
Khi JSON response từ API có property `"Category"` và `"Brand"`, nhưng Model C# có property `CategoryName` và `BrandName`, JSON deserializer không tự động map được → giá trị `null` → hiển thị "N/A"

---

## ✅ Giải pháp

### **Sửa API để trả về đúng tên property**

#### File: `TearGearAPI/Controllers/ProductAPIsController.cs`

**Trước khi sửa (❌ SAI):**
```csharp
[HttpGet]
public async Task<IActionResult> GetProducts()
{
    var products = await _context.productAPIs
        .Include(p => p.Category)
        .Include(p => p.Brand)
        .Select(p => new
        {
            p.Id,
            p.Name,
            p.Description,
            p.ImageUrl,
            p.CategoryId,
            p.BrandId,
            Category = p.Category.CategoryName,   // ❌ Tên property sai
            Brand = p.Brand.BrandName             // ❌ Tên property sai
        })
        .ToListAsync();

    return Ok(products);
}
```

**Sau khi sửa (✅ ĐÚNG):**
```csharp
[HttpGet]
public async Task<IActionResult> GetProducts()
{
    var products = await _context.productAPIs
        .Include(p => p.Category)
        .Include(p => p.Brand)
        .Select(p => new
        {
            p.Id,
            p.Name,
            p.Description,
            p.ImageUrl,
            p.CategoryId,
            p.BrandId,
            CategoryName = p.Category.CategoryName,  // ✅ Đúng tên
            BrandName = p.Brand.BrandName            // ✅ Đúng tên
        })
        .ToListAsync();

    return Ok(products);
}
```

### **Các API endpoints cần sửa:**

1. ✅ **GET** `/api/products` - Get all products
2. ✅ **GET** `/api/products/latest/{count}` - Get latest products
3. ✅ **GET** `/api/products/category/{categoryId}` - Get by category
4. ✅ **GET** `/api/products/brand/{brandId}` - Get by brand
5. ✅ **GET** `/api/products/{id}` - Get product detail

**Tất cả đều phải đổi:**
- `Category = ...` → `CategoryName = ...`
- `Brand = ...` → `BrandName = ...`

---

## 📊 Kết quả sau khi sửa

### JSON Response từ API (trước):
```json
{
  "id": 1,
  "name": "Laptop MSI Thin 15",
  "description": "mạnh mẽ",
  "imageUrl": "...",
  "categoryId": 1,
  "brandId": 1,
  "category": "Laptop",        // ❌ Tên property sai
  "brand": "MSI"               // ❌ Tên property sai
}
```

### JSON Response từ API (sau):
```json
{
  "id": 1,
  "name": "Laptop MSI Thin 15",
  "description": "mạnh mẽ",
  "imageUrl": "...",
  "categoryId": 1,
  "brandId": 1,
  "categoryName": "Laptop",    // ✅ Tên property đúng
  "brandName": "MSI"           // ✅ Tên property đúng
}
```

### Hiển thị trong Admin Panel:
```
Name              | Danh mục | Thương hiệu | Description | Hình ảnh
Laptop MSI Thin 15| Laptop   | MSI         | mạnh mẽ     | [Image]
```

✅ **Không còn "N/A" nữa!**

---

## 🔍 Cách kiểm tra

### 1. **Kiểm tra API Response**
Sử dụng Swagger hoặc Postman:
```
GET https://localhost:7034/api/products
```

**Response phải có:**
```json
[
  {
    "id": 1,
    "name": "Laptop MSI Thin 15",
    "categoryName": "Laptop",    // ✅ Phải có field này
    "brandName": "MSI"           // ✅ Phải có field này
  }
]
```

### 2. **Kiểm tra trong Browser**
1. Mở Developer Tools (F12)
2. Tab **Network**
3. Refresh trang Admin Products
4. Click vào request `GetProducts`
5. Xem **Response** tab
6. Kiểm tra có `categoryName` và `brandName` không

### 3. **Kiểm tra Model Binding**
Thêm breakpoint trong `AdminProductsController.cs`:
```csharp
public async Task<IActionResult> Index()
{
    var products = await _apiService.GetAsync<List<Product>>("api/products");
    
    // ✅ Breakpoint ở đây
    // Kiểm tra products[0].CategoryName
    // Kiểm tra products[0].BrandName
    
    return View(products ?? new List<Product>());
}
```

---

## 📝 Lưu ý quan trọng

### 1. **Naming Convention phải khớp**
- Property trong API response: `CategoryName`, `BrandName`
- Property trong C# Model: `CategoryName`, `BrandName`
- **Phải giống hệt** (case-sensitive!)

### 2. **Các API khác cũng cần kiểm tra**
- `/api/categories` → Nếu có nested object thì cũng phải đổi tên property
- `/api/brands` → Tương tự
- Bất kỳ API nào trả về Product entity

### 3. **Alternative: Dùng JsonProperty Attribute**
Nếu không muốn đổi API, có thể dùng attribute:
```csharp
public class Product
{
    // ...
    
    [JsonProperty("Category")]  // Map từ "Category" trong JSON
    public string? CategoryName { get; set; }
    
    [JsonProperty("Brand")]     // Map từ "Brand" trong JSON
    public string? BrandName { get; set; }
}
```

Nhưng cách này **KHÔNG KHUYẾN KHÍCH** vì:
- Làm code khó hiểu
- Phải thêm dependency `Newtonsoft.Json`
- Dễ gây confuse cho developer khác

---

## 🚀 Các bước triển khai

1. ✅ **Sửa API Controller** (TearGearAPI/Controllers/ProductAPIsController.cs)
   - Đổi `Category` → `CategoryName`
   - Đổi `Brand` → `BrandName`
   
2. ✅ **Build API Project**
   ```bash
   cd TearGearAPI
   dotnet build
   ```

3. ✅ **Restart API**
   ```bash
   dotnet run
   ```

4. ✅ **Test trong Swagger**
   - Mở `https://localhost:7034/swagger`
   - Test endpoint `/api/products`
   - Xem response có `categoryName` và `brandName`

5. ✅ **Refresh MVC Admin Panel**
   - Mở `https://localhost:7206/Admin/AdminProducts`
   - Kiểm tra danh mục và thương hiệu hiển thị đúng

---

## 🎯 Checklist

- [x] Sửa API `GetProducts()`
- [x] Sửa API `GetLatestProducts()`
- [x] Sửa API `GetProductsByCategory()`
- [x] Sửa API `GetProductsByBrand()`
- [x] Sửa API `GetProduct(id)`
- [x] Build API thành công
- [x] Test API response trong Swagger
- [x] Kiểm tra MVC hiển thị đúng

---

## 📞 Troubleshooting

### Vẫn hiển thị "N/A"?

1. **Kiểm tra API có chạy không?**
   ```bash
   curl https://localhost:7034/api/products
   ```

2. **Kiểm tra Response JSON**
   - Có field `categoryName` không?
   - Có field `brandName` không?

3. **Kiểm tra trong database**
   ```sql
   SELECT p.*, c.CategoryName, b.BrandName
   FROM ProductAPIs p
   LEFT JOIN CategoryAPIs c ON p.CategoryId = c.CategoryId
   LEFT JOIN BrandAPIs b ON p.BrandId = b.BrandId;
   ```

4. **Clear cache trình duyệt**
   - Ctrl + Shift + Delete
   - Xóa cached images and files

5. **Hard refresh**
   - Ctrl + F5 (Windows)
   - Cmd + Shift + R (Mac)

---

**Ngày cập nhật:** 25/02/2026  
**Version:** 1.0  
**Trạng thái:** ✅ Đã khắc phục
