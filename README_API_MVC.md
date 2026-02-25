# TechGear - H??ng d?n liên k?t MVC và API

## T?ng quan
D? án TechGear ?ã ???c c?u hình ?? tách bi?t gi?a:
- **TearGearAPI**: Backend API (ASP.NET Core Web API)
- **TechGear**: Frontend MVC (ASP.NET Core MVC)

## C?u trúc d? án

### TearGearAPI (Backend)
- **Port**: https://localhost:3636
- **Controllers**:
  - `ProductAPIsController`: Qu?n lý s?n ph?m
  - `CategoryAPIsController`: Qu?n lý danh m?c
  - `BrandAPIsController`: Qu?n lý th??ng hi?u
  - `OrderAPIsController`: Qu?n lý ??n hàng
  - `OrderDetailAPIsController`: Qu?n lý chi ti?t ??n hàng
  - `PaymentAPIsController`: Qu?n lý thanh toán

### TechGear (Frontend)
- **Services**:
  - `IApiService`: Interface ??nh ngh?a các ph??ng th?c g?i API
  - `ApiService`: Implementation g?i API t? Backend

- **Controllers**:
  - `HomeController`: Trang ch?, s? d?ng `IApiService`
  - `ProductsController`: Trang s?n ph?m, s? d?ng `IApiService`

- **ViewComponents**:
  - `CategoryBarViewComponent`: Hi?n th? danh sách danh m?c trong menu

## API Endpoints

### Products API
```
GET /api/ProductAPIs                    - L?y t?t c? s?n ph?m
GET /api/ProductAPIs/latest/{count}     - L?y s?n ph?m m?i nh?t
GET /api/ProductAPIs/{id}               - L?y chi ti?t s?n ph?m
GET /api/ProductAPIs/category/{id}      - L?y s?n ph?m theo danh m?c
GET /api/ProductAPIs/brand/{id}         - L?y s?n ph?m theo th??ng hi?u
```

### Categories API
```
GET /api/CategoryAPIs                   - L?y t?t c? danh m?c
GET /api/CategoryAPIs/{id}              - L?y chi ti?t danh m?c
```

### Brands API
```
GET /api/BrandAPIs                      - L?y t?t c? th??ng hi?u
GET /api/BrandAPIs/{id}                 - L?y chi ti?t th??ng hi?u
```

## Cách s? d?ng ApiService

### 1. Trong Controller
```csharp
public class YourController : Controller
{
    private readonly IApiService _apiService;

    public YourController(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _apiService.GetProductsAsync();
        return View(products);
    }
}
```

### 2. Các ph??ng th?c có s?n
```csharp
// Products
await _apiService.GetProductsAsync();
await _apiService.GetLatestProductsAsync(12);
await _apiService.GetProductByIdAsync(id);
await _apiService.GetProductsByCategoryAsync(categoryId);
await _apiService.GetProductsByBrandAsync(brandId);

// Categories
await _apiService.GetCategoriesAsync();
await _apiService.GetCategoryByIdAsync(id);

// Brands
await _apiService.GetBrandsAsync();
await _apiService.GetBrandByIdAsync(id);
```

## C?u hình

### appsettings.json (TechGear)
```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:3636/api"
  }
}
```

### Program.cs (TechGear)
```csharp
// ?ã ???c c?u hình:
builder.Services.AddHttpClient("TechGearAPI", client =>
{
    var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"];
    client.BaseAddress = new Uri(apiBaseUrl);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddScoped<IApiService, ApiService>();
```

### Program.cs (TearGearAPI)
```csharp
// CORS ?ã ???c c?u hình cho phép MVC g?i API:
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowMVC",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Trong Configure:
app.UseCors("AllowMVC");
```

## Ch?y d? án

### Cách 1: Ch?y t?ng d? án riêng l?
1. M? 2 terminal
2. Terminal 1: Ch?y API
   ```bash
   cd TearGearAPI
   dotnet run
   ```
3. Terminal 2: Ch?y MVC
   ```bash
   cd TechGear
   dotnet run
   ```

### Cách 2: Configure Multiple Startup Projects trong Visual Studio
1. Right-click Solution ? Properties
2. Ch?n "Multiple startup projects"
3. Set Action = "Start" cho c? TearGearAPI và TechGear
4. Click OK và nh?n F5

## L?u ý quan tr?ng

### 1. Kh?i ??ng th? t?
- **Luôn kh?i ??ng TearGearAPI tr??c** vì TechGear c?n g?i API
- ??m b?o API ?ang ch?y ? port 3636

### 2. Models
- Models trong TechGear là ViewModels, ch? dùng ?? hi?n th?
- Models trong TearGearAPI là Entity Models, k?t n?i v?i Database
- Tên properties ph?i kh?p nhau (ho?c dùng PropertyNameCaseInsensitive = true)

### 3. Error Handling
- ApiService ?ã x? lý l?i c? b?n
- Tr? v? empty list ho?c null n?u có l?i
- Log errors trong ILogger

### 4. B?o m?t (T??ng lai)
- Hi?n t?i API public, không c?n authentication
- Có th? thêm JWT authentication sau này
- C?n thêm API key ho?c OAuth2 cho production

## M? r?ng thêm API

### 1. T?o Controller m?i trong TearGearAPI
```csharp
[Route("api/[controller]")]
[ApiController]
public class YourNewAPIsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public YourNewAPIsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<YourModel>>> GetData()
    {
        return await _context.YourTable.ToListAsync();
    }
}
```

### 2. Thêm method vào IApiService
```csharp
Task<List<YourModel>> GetYourDataAsync();
```

### 3. Implement trong ApiService
```csharp
public async Task<List<YourModel>> GetYourDataAsync()
{
    return await GetAsync<List<YourModel>>("/YourNewAPIs") ?? new List<YourModel>();
}
```

## Troubleshooting

### L?i: Cannot connect to API
- Ki?m tra TearGearAPI ?ã ch?y ch?a
- Ki?m tra port trong appsettings.json kh?p v?i launchSettings.json
- Ki?m tra CORS ?ã ???c c?u hình

### L?i: Deserialize JSON failed
- Ki?m tra tên properties trong Model kh?p v?i API response
- Thêm PropertyNameCaseInsensitive = true

### L?i: 404 Not Found
- Ki?m tra route c?a API Controller
- Ki?m tra endpoint trong ApiService

## Contact
- Email: support@techgear.vn
- Hotline: 1900 6750

## S? d?ng ViewComponents

### CategoryBarViewComponent
ViewComponent này ???c s? d?ng ?? hi?n th? danh sách categories trong navigation bar.

**Cách s? d?ng trong View:**
```razor
@await Component.InvokeAsync("CategoryBar")
```

**V? trí file:**
- Component: `TechGear/ViewComponents/CategoryBarViewComponent.cs`
- View: `TechGear/Views/Shared/Components/CategoryBar/Default.cshtml`

**T?o ViewComponent m?i:**
```csharp
// 1. T?o ViewComponent class
public class YourViewComponent : ViewComponent
{
    private readonly IApiService _apiService;

    public YourViewComponent(IApiService apiService)
    {
        _apiService = apiService;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var data = await _apiService.GetDataAsync();
        return View(data);
    }
}

// 2. T?o View t?i: Views/Shared/Components/Your/Default.cshtml
