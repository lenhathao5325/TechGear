# ?? H??NG D?N TEST API ENDPOINTS

## ? V?N ?? HI?N T?I

T? log c?a b?n:
```
Line 12: Sending HTTP request GET https://localhost:3636/products/latest/12
Line 19: fail: TechGear.Services.ApiService[0]
         API call to /products/latest/12 failed with status code: NotFound
```

**Nguyên nhân:** API Project (TearGearAPI) ch?a ???c ch?y ho?c ?ang ch?y sai port.

---

## ? GI?I PHÁP - LÀM THEO TH? T?

### **B??C 1: Kh?i ??ng API Project TR??C**

#### Option 1: Ch?y t? Visual Studio
1. Click chu?t ph?i vào project **TearGearAPI**
2. Ch?n **"Set as Startup Project"**
3. Ch?n profile **"https"** trong dropdown
4. Nh?n **F5** ho?c click **Start**
5. Swagger UI s? m? t?i: `https://localhost:3636/swagger`

#### Option 2: Ch?y t? Terminal
```powershell
cd TearGearAPI
dotnet run --launch-profile https
```

**Ki?m tra:** B?n s? th?y log:
```
Now listening on: https://localhost:3636
Now listening on: http://localhost:5089
```

---

### **B??C 2: Test API tr?c ti?p qua Swagger**

M? browser: `https://localhost:3636/swagger`

Test các endpoints sau:

#### 1. GET `/api/products`
- Click **Try it out** ? **Execute**
- **Expected:** Status 200, tr? v? list products

#### 2. GET `/api/products/latest/12`
- Click **Try it out** ? Nh?p count = `12` ? **Execute**
- **Expected:** Status 200, tr? v? 12 s?n ph?m m?i nh?t

#### 3. GET `/api/categories`
- Click **Try it out** ? **Execute**
- **Expected:** Status 200, tr? v? list categories

#### 4. GET `/api/brands`
- Click **Try it out** ? **Execute**
- **Expected:** Status 200, tr? v? list brands

**N?u t?t c? tr? v? 200 OK** ? API ho?t ??ng t?t ?

---

### **B??C 3: Ki?m tra Database có d? li?u**

N?u API tr? v? `[]` (m?ng r?ng), ngh?a là **database ch?a có d? li?u**.

#### A. Ki?m tra Connection String

File: `TearGearAPI/appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=...;..."
  }
}
```

**N?u thi?u connection string**, hãy thêm vào.

#### B. Ch?y Migration

```powershell
cd TearGearAPI
dotnet ef database update
```

**Expected output:**
```
Applying migration 'XXXXXXXX_InitialCreate'.
Done.
```

#### C. Seed d? li?u m?u (n?u có)

Ki?m tra xem có migration seed data không:
```
Migrations/
  - 20260206064643_Seed5Data.cs  ? Có seed data
```

N?u có, database ?ã ???c seed khi ch?y migration.

---

### **B??C 4: Kh?i ??ng c? 2 Projects cùng lúc**

#### Option 1: Multiple Startup Projects (Visual Studio)

1. Click chu?t ph?i vào **Solution** (TechGearBaoQTK)
2. Ch?n **Properties**
3. Ch?n **Multiple startup projects**
4. Set c? **TearGearAPI** và **TechGear** = **Start**
5. Nh?n **OK**
6. Nh?n **F5**

#### Option 2: 2 Terminal Windows

**Terminal 1 - API:**
```powershell
cd TearGearAPI
dotnet run --launch-profile https
```

**Terminal 2 - MVC:**
```powershell
cd TechGear
dotnet run --launch-profile https
```

---

### **B??C 5: Ki?m tra MVC có g?i ???c API không**

Sau khi c? 2 projects ?ang ch?y:

1. M? browser: `https://localhost:7002` (TechGear)
2. Vào trang Home
3. M? **Developer Tools** (F12)
4. Vào tab **Network**
5. Refresh trang
6. Ki?m tra các API calls:
   - `https://localhost:3636/api/products/latest/12` ? Status **200** ?
   - `https://localhost:3636/api/categories` ? Status **200** ?

**N?u v?n 404:**
- Ki?m tra API có ?ang ch?y không
- Ki?m tra port có ?úng không
- Ki?m tra firewall/antivirus có block không

---

### **B??C 6: S? d?ng POSTMAN ?? test API (Tùy ch?n)**

N?u mu?n test chi ti?t h?n, b?n có th? dùng POSTMAN:

#### A. Cài ??t POSTMAN
- T?i và cài ??t t?i: [POSTMAN Download](https://www.postman.com/downloads/)

#### B. Import API Collection
- T?i file [TearGearAPI.postman_collection.json](link-to-file) v? máy
- M? POSTMAN, ch?n **Import** ? **Upload Files**
- Ch?n file v?a t?i v?

#### C. Test các API
- M? collection v?a import
- Ch?n m?t API b?t k? ?? test
- Nh?n **Send**

**L?u ý:** Khi dùng POSTMAN, b?n c?n thêm header `Authorization` cho các API c?n xác th?c:
```
Authorization: Bearer {your_token}
```

---

## ?? DEBUG LOG

N?u v?n l?i, thêm logging vào ApiService ?? xem URL ??y ??:

```csharp
private async Task<T?> GetAsync<T>(string endpoint)
{
    try
    {
        var client = CreateClient();
        
        // ?? DEBUG: Log URL ??y ??
        var fullUrl = $"{client.BaseAddress}{endpoint}";
        _logger.LogInformation($"?? Calling API: {fullUrl}");
        
        var response = await client.GetAsync(endpoint);
        
        // ?? DEBUG: Log status code
        _logger.LogInformation($"?? Response Status: {response.StatusCode}");
        
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(content, _jsonOptions);
        }
        // ...existing code...
    }
    // ...existing code...
}
```

---

## ?? CHECKLIST

?ánh d?u khi hoàn thành:

- [ ] TearGearAPI ?ang ch?y trên port 3636
- [ ] Swagger UI m? ???c t?i `https://localhost:3636/swagger`
- [ ] Test endpoint `/api/products/latest/12` qua Swagger ? 200 OK
- [ ] Test endpoint `/api/categories` qua Swagger ? 200 OK
- [ ] Database có d? li?u (products, categories, brands)
- [ ] TechGear MVC ?ang ch?y trên port 7002
- [ ] MVC g?i ???c API và hi?n th? d? li?u

---

## ?? N?U V?N L?I

Hãy cung c?p:
1. Screenshot c?a Swagger UI (`https://localhost:3636/swagger`)
2. Response khi test endpoint `/api/products/latest/12` trong Swagger
3. Full log t? c? 2 projects (API và MVC)
4. Screenshot network tab trong browser khi truy c?p MVC

---

**Next Steps:**
1. Ch?y TearGearAPI tr??c
2. Test qua Swagger
3. Sau ?ó m?i ch?y TechGear MVC