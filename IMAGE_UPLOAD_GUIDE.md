# 📸 HƯỚNG DẪN UPLOAD ẢNH SẢN PHẨM

## ❌ Vấn đề trước khi sửa
- Upload ảnh trong form Create/Edit nhưng không được lưu
- Cột "Hình ảnh" hiển thị trống trong danh sách sản phẩm
- ImageFile được gửi lên server nhưng không xử lý

## ✅ Giải pháp đã triển khai

### 1. **Tạo Upload API Endpoint**
**File:** `TearGearAPI/Controllers/UploadController.cs`

```csharp
[Route("api/[controller]")]
[ApiController]
public class UploadController : ControllerBase
{
    // POST api/upload/image?folder=products
    [HttpPost("image")]
    public async Task<IActionResult> UploadImage(IFormFile file, string folder = "techgear")
    {
        // Upload to Cloudinary
        var imageUrl = await _cloudinaryService.UploadImageAsync(file, folder);
        return Ok(new { success = true, imageUrl });
    }

    // POST api/upload/images?folder=products  
    [HttpPost("images")]
    public async Task<IActionResult> UploadImages(List<IFormFile> files, string folder = "techgear")
    {
        // Upload multiple images
    }
}
```

**Features:**
- ✅ Upload single image
- ✅ Upload multiple images
- ✅ Validate file type (JPG, PNG, GIF, WEBP)
- ✅ Validate file size (max 5MB)
- ✅ Return image URL from Cloudinary

---

### 2. **Cập nhật AdminProductsController**

#### **Create Action**
```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(ProductVM viewModel)
{
    if (ModelState.IsValid)
    {
        string imageUrl = null;

        // 1. Upload image to Cloudinary first
        if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
        {
            using var formData = new MultipartFormDataContent();
            using var streamContent = new StreamContent(viewModel.ImageFile.OpenReadStream());
            streamContent.Headers.ContentType = new MediaTypeHeaderValue(viewModel.ImageFile.ContentType);
            formData.Add(streamContent, "file", viewModel.ImageFile.FileName);

            var uploadResponse = await _apiService.PostAsync<dynamic>("api/upload/image?folder=products", formData);
            
            if (uploadResponse?.success == true)
            {
                imageUrl = uploadResponse.imageUrl.ToString();
            }
        }

        // 2. Create product with image URL
        var productDto = new 
        { 
            Name = viewModel.Name,
            Description = viewModel.Description,
            CategoryId = viewModel.CategoryId,
            BrandId = viewModel.BrandId,
            ImageUrl = imageUrl  // ✅ Save Cloudinary URL
        };
        
        await _apiService.PostAsync<Product>("api/products", productDto);
        return RedirectToAction(nameof(Index));
    }
    return View(viewModel);
}
```

#### **Edit Action**
```csharp
[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(int id, ProductVM viewModel)
{
    string imageUrl = viewModel.ImageUrl; // Keep existing image

    // Upload new image if provided
    if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
    {
        // Same upload logic as Create
        // ...
        imageUrl = uploadResponse.imageUrl.ToString();
    }

    var productDto = new 
    { 
        // ...
        ImageUrl = imageUrl  // Use new image or keep old one
    };
    
    await _apiService.PutAsync($"api/products/{id}", productDto);
    return RedirectToAction(nameof(Index));
}
```

---

### 3. **Cải tiến Create.cshtml View**

**Features mới:**
- ✅ Preview ảnh trước khi upload
- ✅ Validate file type client-side
- ✅ Validate file size (max 5MB)
- ✅ UI/UX đẹp hơn với Bootstrap 5
- ✅ Responsive design

**Code highlights:**
```razor
<!-- Image input with accept -->
<input asp-for="ImageFile" type="file" class="form-control" 
       id="imageInput" accept="image/*" />

<!-- Image preview -->
<div id="imagePreview" style="display: none;">
    <img id="previewImg" src="" class="img-fluid rounded" />
</div>

<!-- JavaScript validation & preview -->
<script>
$('#imageInput').on('change', function(e) {
    const file = e.target.files[0];
    
    // Validate size
    if (file.size > 5 * 1024 * 1024) {
        alert('File không được vượt quá 5MB!');
        return;
    }
    
    // Show preview
    const reader = new FileReader();
    reader.onload = function(e) {
        $('#previewImg').attr('src', e.target.result);
        $('#imagePreview').show();
    };
    reader.readAsDataURL(file);
});
</script>
```

---

## 📊 Flow hoạt động

### **Tạo sản phẩm mới:**
```
1. User chọn ảnh trong form Create
   ↓
2. Client-side validation (size, type)
   ↓
3. Submit form → AdminProductsController.Create()
   ↓
4. Upload ImageFile → API /api/upload/image
   ↓
5. API upload lên Cloudinary
   ↓
6. API trả về imageUrl
   ↓
7. Save Product với imageUrl vào database
   ↓
8. Redirect về Index → Hiển thị ảnh từ Cloudinary
```

### **Sửa sản phẩm:**
```
1. Load product hiện tại (có ImageUrl cũ)
   ↓
2. User có thể:
   - Giữ ảnh cũ (không chọn file mới)
   - Đổi ảnh mới (chọn file mới)
   ↓
3. Submit form:
   - Nếu có file mới → Upload lên Cloudinary → Lấy URL mới
   - Nếu không → Giữ ImageUrl cũ
   ↓
4. Update Product với ImageUrl (mới hoặc cũ)
```

---

## 🔍 Kiểm tra

### 1. **Test Upload API**
```bash
# Test với Postman hoặc Swagger
POST https://localhost:7034/api/upload/image?folder=products
Content-Type: multipart/form-data

Body:
- file: [Select image file]
```

**Expected Response:**
```json
{
  "success": true,
  "message": "Image uploaded successfully",
  "imageUrl": "https://res.cloudinary.com/...jpg"
}
```

### 2. **Test Create Product**
1. Mở `/Admin/AdminProducts/Create`
2. Nhập thông tin sản phẩm
3. Chọn ảnh → Preview hiển thị ngay
4. Click "Lưu sản phẩm"
5. Redirect về Index → Kiểm tra ảnh hiển thị

### 3. **Test Edit Product**
1. Mở `/Admin/AdminProducts/Edit/1`
2. Form load với ảnh cũ
3. Có thể:
   - Giữ ảnh cũ → Submit → Ảnh không đổi
   - Chọn ảnh mới → Submit → Ảnh được cập nhật

### 4. **Kiểm tra Database**
```sql
SELECT Id, Name, ImageUrl 
FROM ProductAPIs;
```

**Expected:**
```
Id | Name             | ImageUrl
1  | Laptop MSI Thin  | https://res.cloudinary.com/.../abc123.jpg
```

---

## 🚨 Validation Rules

### **Client-side (JavaScript):**
- ✅ File type: JPG, PNG, GIF, WEBP
- ✅ File size: Max 5MB
- ✅ Preview before upload

### **Server-side (API):**
- ✅ File type: `.jpg`, `.jpeg`, `.png`, `.gif`, `.webp`
- ✅ File size: Max 5MB (5 * 1024 * 1024 bytes)
- ✅ Null check: `file != null && file.Length > 0`

---

## 📝 Lưu ý quan trọng

### 1. **Cloudinary Configuration**
Đảm bảo trong `appsettings.json` (API) có:
```json
{
  "CloudinarySettings": {
    "CloudName": "your-cloud-name",
    "ApiKey": "your-api-key",
    "ApiSecret": "your-api-secret"
  }
}
```

### 2. **Folder Structure trong Cloudinary**
- Products: `techgear/products/`
- Categories: `techgear/categories/`
- Brands: `techgear/brands/`

### 3. **Error Handling**
- Upload failed → Hiển thị TempData["Error"]
- Không ảnh → ImageUrl = null (OK, không bắt buộc)
- File quá lớn → Alert client-side + Reject server-side

### 4. **Performance**
- Upload ảnh trước → Tạo product sau
- Nếu upload fail → Không tạo product
- Edit: Chỉ upload khi có file mới

---

## 🐛 Troubleshooting

### ❌ Ảnh không hiển thị
**Nguyên nhân:**
- ImageUrl = null trong database
- Cloudinary URL không hợp lệ
- CORS issue

**Giải pháp:**
1. Kiểm tra database: `SELECT ImageUrl FROM ProductAPIs`
2. Kiểm tra Cloudinary dashboard
3. Kiểm tra network tab trong browser

### ❌ Upload failed
**Nguyên nhân:**
- Cloudinary credentials sai
- File size quá lớn
- Network timeout

**Giải pháp:**
1. Kiểm tra `appsettings.json`
2. Test với file nhỏ hơn
3. Tăng timeout trong `ApiService`

### ❌ Preview không hiển thị
**Nguyên nhân:**
- JavaScript error
- jQuery không load
- File reader không support

**Giải pháp:**
1. F12 → Console → Kiểm tra error
2. Đảm bảo jQuery đã load
3. Test trên browser khác

---

## 🎯 Checklist

- [x] Tạo UploadController trong API
- [x] Cập nhật AdminProductsController.Create()
- [x] Cập nhật AdminProductsController.Edit()
- [x] Cải tiến Create.cshtml view
- [x] Thêm image preview
- [x] Thêm client-side validation
- [x] Thêm server-side validation
- [x] Test upload single image
- [x] Test create product with image
- [x] Test edit product (keep/change image)
- [x] Kiểm tra hiển thị trong Index

---

## 📞 API Endpoints

### Upload single image
```
POST /api/upload/image?folder=products
Content-Type: multipart/form-data
Body: file=[binary]
```

### Upload multiple images
```
POST /api/upload/images?folder=products
Content-Type: multipart/form-data
Body: files=[binary array]
```

### Create product
```
POST /api/products
Content-Type: application/json
Body: {
  "name": "Laptop MSI",
  "description": "...",
  "categoryId": 1,
  "brandId": 1,
  "imageUrl": "https://res.cloudinary.com/..."
}
```

---

**Ngày cập nhật:** 25/02/2026  
**Version:** 2.0  
**Trạng thái:** ✅ Upload ảnh hoạt động đầy đủ
