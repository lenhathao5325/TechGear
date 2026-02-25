# ? QUICK START GUIDE - FIX 404 ERROR

## ?? M?C TIÊU
Kh?i ??ng TearGearAPI và TechGear MVC ?? ch?y cùng nhau.

---

## ?? CHU?N B?

### 1. Connection String ?ã ???c thêm vào
? File `TearGearAPI/appsettings.json` ?ã có:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TechGearAPIDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
```

**N?u b?n dùng SQL Server khác**, hãy s?a connection string cho phù h?p.

---

## ?? B??C 1: SETUP DATABASE

M? Terminal trong Visual Studio (View ? Terminal):

```powershell
# Di chuy?n vào th? m?c API
cd TearGearAPI

# Apply migrations vào database
dotnet ef database update
```

**Expected Output:**
```
Applying migration 'XXXXXXXX_InitialCreate'.
Done.
```

---

## ?? B??C 2: KH?I ??NG C? 2 PROJECTS

### ? RECOMMENDED: Multiple Startup Projects

1. Click chu?t ph?i vào **Solution "TechGearBaoQTK"** trong Solution Explorer
2. Ch?n **"Set Startup Projects..."** (ho?c **"Configure Startup Projects..."**)
3. Ch?n **"Multiple startup projects"**
4. Set c? 2 projects:
   - **TearGearAPI**: Action = **Start**
   - **TechGear**: Action = **Start**
5. ??m b?o **TearGearAPI** ? trên **TechGear** (?? API start tr??c)
6. Click **OK**
7. Nh?n **F5** ?? ch?y

---

## ?? B??C 3: XÁC NH?N API HO?T ??NG

### A. Ki?m tra Swagger
Browser s? t? ??ng m?: `https://localhost:3636/swagger`

Test các endpoints:

1. **GET `/api/products/latest/{count}`**
   - Click **"Try it out"**
   - Nh?p `count` = `12`
   - Click **"Execute"**
   - ? **Expected:** Status **200**, có data ho?c `[]`

2. **GET `/api/categories`**
   - Click **"Try it out"** ? **"Execute"**
   - ? **Expected:** Status **200**

3. **GET `/api/brands`**
   - Click **"Try it out"** ? **"Execute"**
   - ? **Expected:** Status **200**

### B. Ki?m tra Output Window
1. M? **View** ? **Output**
2. Ch?n dropdown: **"TearGearAPI"** ho?c **"Debug"**
3. Tìm dòng:
   ```
   Now listening on: https://localhost:3636
   ```
   ? API ?ang ch?y!

---

## ?? B??C 4: XÁC NH?N MVC HO?T ??NG

### A. M? TechGear trong browser
Browser th? 2 s? m?: `https://localhost:7002`

### B. Ki?m tra Developer Tools
1. Nh?n **F12** ?? m? DevTools
2. Tab **Console**:
   - ? Không có l?i màu ??
3. Tab **Network**:
   - Refresh trang (**Ctrl+R**)
   - Tìm requests ??n `localhost:3636`:
     ```
     ? GET https://localhost:3636/api/products/latest/12 ? Status: 200
     ? GET https://localhost:3636/api/categories ? Status: 200
     ```

### C. Ki?m tra Output Window c?a MVC
1. Ch?n dropdown: **"TechGear"** ho?c **"Debug"**
2. Tìm các dòng log:
   ```
   info: System.Net.Http.HttpClient.TechGearAPI.LogicalHandler[100]
         Start processing HTTP request GET https://localhost:3636/api/products/latest/12
   info: System.Net.Http.HttpClient.TechGearAPI.ClientHandler[101]
         Sending HTTP request GET https://localhost:3636/api/products/latest/12
   info: System.Net.Http.HttpClient.TechGearAPI.ClientHandler[101]
         Received HTTP response headers after XXms - 200
   ```
   ? **Status 200** = Thành công!

---

## ? TROUBLESHOOTING

### ? Problem 1: "Unable to connect to SQL Server"

**Gi?i pháp 1:** Ki?m tra SQL Server ?ang ch?y
```powershell
Get-Service | Where-Object {$_.Name -like "*SQL*"}
```

**Gi?i pháp 2:** Dùng InMemory Database (test nhanh)

Edit `TearGearAPI/Program.cs`:
```csharp
// Comment out UseSqlServer
// builder.Services.AddDbContext<ApplicationDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
// );

// Add InMemoryDatabase
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("TechGearTestDB")
);
```

### ? Problem 2: API tr? v? `[]` (empty array)

**Nguyên nhân:** Database ch?a có d? li?u

**Gi?i pháp:** Ki?m tra migrations có seed data không
```powershell
cd TearGearAPI
dotnet ef migrations list
```

N?u có migration `Seed5Data`, ?ã có data seeding t? ??ng.

**Thêm data test nhanh:** Dùng Swagger ?? POST data m?u.

### ? Problem 3: MVC v?n nh?n 404

**Checklist:**

1. ? API ?ang ch?y trên port 3636
2. ? `TechGear/appsettings.json` có:
   ```json
   "ApiSettings": {
     "BaseUrl": "https://localhost:3636/api"
   }
   ```
   **CHÚ Ý:** Ph?i có `/api` ? cu?i!

3. ? Ki?m tra URL ??y ?? trong log:
   - Ph?i là: `https://localhost:3636/api/products/...`
   - Không ph?i: `https://localhost:3636/products/...`

**Debug b?ng cách thêm log:**

Edit `TechGear/Services/ApiService.cs`:
```csharp
private async Task<T?> GetAsync<T>(string endpoint)
{
    var client = CreateClient();
    
    // ?? DEBUG LOG
    var fullUrl = $"{client.BaseAddress}{endpoint}";
    _logger.LogWarning($"?? Calling: {fullUrl}");
    Console.WriteLine($"?? Calling: {fullUrl}");
    
    var response = await client.GetAsync(endpoint);
    
    // ?? RESPONSE LOG
    _logger.LogWarning($"?? Response: {response.StatusCode}");
    Console.WriteLine($"?? Response: {response.StatusCode}");
    
    // ...existing code...
}
```

Restart MVC và xem Output window.

### ? Problem 4: "Port already in use"

**Gi?i pháp:** ??i port trong `launchSettings.json`

Example:
```json
"applicationUrl": "https://localhost:3637;http://localhost:5090"
```

### ? Problem 5: Certificate SSL error

**Gi?i pháp:** Trust development certificate
```powershell
dotnet dev-certs https --trust
```

---

## ? SUCCESS INDICATORS

B?n thành công khi th?y:

### Console/Output Window (API):
```
? Now listening on: https://localhost:3636
? Now listening on: http://localhost:5089
? Application started.
```

### Swagger UI:
```
? https://localhost:3636/swagger loads
? GET /api/products/latest/12 ? 200 OK
? GET /api/categories ? 200 OK
```

### Console/Output Window (MVC):
```
? Now listening on: https://localhost:7002
? GET https://localhost:3636/api/products/latest/12 ? 200 OK
```

### Browser (MVC):
```
? Homepage loads
? Products displayed (ho?c empty n?u ch?a có data)
? No 404 errors in Console (F12)
```

---

## ?? QUICK CHECKLIST

Copy và ?ánh d?u khi hoàn thành:

```
[ ] SQL Server/LocalDB ?ang ch?y
[ ] dotnet ef database update ? Success
[ ] Set Multiple Startup Projects (TearGearAPI + TechGear)
[ ] F5 ? C? 2 apps start
[ ] Swagger UI m?: https://localhost:3636/swagger
[ ] Test API endpoint qua Swagger ? 200 OK
[ ] MVC homepage m?: https://localhost:7002
[ ] F12 Network tab ? API calls return 200
[ ] No errors in Console
```

---

## ?? RELATED DOCS

- Xem chi ti?t API endpoints: `API_MVC_CONNECTION_REPORT.md`
- Test manual endpoints: `TEST_API_ENDPOINTS.md`

---

## ?? C?N H? TR??

N?u v?n g?p l?i, cung c?p:
1. Screenshot Output window (c? API và MVC)
2. Screenshot Swagger UI test result
3. Screenshot Browser DevTools Network tab
4. Full error message

**Good luck! ??**
