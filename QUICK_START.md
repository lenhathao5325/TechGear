# ?? H??ng d?n ch?y TechGear

## ? ?ã s?a l?i

L?i **"A view component named 'CategoryBar' could not be found"** ?ã ???c kh?c ph?c b?ng cách:
1. ? T?o `CategoryBarViewComponent.cs`
2. ? View component ?ã ???c k?t n?i v?i API ?? l?y danh sách categories
3. ? C?p nh?t `_ViewImports.cshtml` ?? nh?n di?n ViewComponent

## ?? Checklist tr??c khi ch?y

### 1. Ki?m tra Database
??m b?o database ?ã có d? li?u:
```sql
-- Ki?m tra trong SQL Server
SELECT * FROM categoryAPIs;
SELECT * FROM brandAPIs;
SELECT * FROM productAPIs;
```

N?u ch?a có d? li?u, c?n insert d? li?u m?u ho?c ch?y migration/seed data.

### 2. Ki?m tra Connection String
File: `TearGearAPI/appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=TechGear;User Id=sa;Password=123456;TrustServerCertificate=True;"
  }
}
```

### 3. Ki?m tra API URL
File: `TechGear/appsettings.json`
```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:3636/api"
  }
}
```

## ?? Cách ch?y d? án

### Option 1: Visual Studio (Khuy?n ngh?)
1. **M? Solution** `TechGearBaoQTK.sln`
2. **Right-click Solution** ? Properties
3. Ch?n **"Multiple startup projects"**
4. Set c? **TearGearAPI** và **TechGear** = **"Start"**
5. ??m b?o **TearGearAPI ? trên TechGear** trong danh sách
6. Click **OK**
7. Nh?n **F5** ho?c click Start

### Option 2: Terminal (2 c?a s?)

**Terminal 1 - Kh?i ??ng API:**
```bash
cd TearGearAPI
dotnet run
```
??i ??n khi th?y: `Now listening on: https://localhost:3636`

**Terminal 2 - Kh?i ??ng MVC:**
```bash
cd TechGear
dotnet run
```

### Option 3: Visual Studio Code

**File: `.vscode/launch.json` (T?o n?u ch?a có)**
```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": "API",
      "type": "coreclr",
      "request": "launch",
      "preLaunchTask": "build-api",
      "program": "${workspaceFolder}/TearGearAPI/bin/Debug/net8.0/TearGearAPI.dll",
      "cwd": "${workspaceFolder}/TearGearAPI",
      "stopAtEntry": false,
      "serverReadyAction": {
        "pattern": "\\bNow listening on:\\s+(https?://\\S+)"
      }
    },
    {
      "name": "MVC",
      "type": "coreclr",
      "request": "launch",
      "preLaunchTask": "build-mvc",
      "program": "${workspaceFolder}/TechGear/bin/Debug/net8.0/TechGear.dll",
      "cwd": "${workspaceFolder}/TechGear",
      "stopAtEntry": false,
      "serverReadyAction": {
        "action": "openExternally",
        "pattern": "\\bNow listening on:\\s+(https?://\\S+)"
      }
    }
  ],
  "compounds": [
    {
      "name": "TechGear Full",
      "configurations": ["API", "MVC"]
    }
  ]
}
```

Sau ?ó ch?n **"TechGear Full"** trong Debug panel và nh?n F5.

## ?? Test API tr??c khi ch?y MVC

1. Kh?i ??ng **TearGearAPI** tr??c
2. M? browser và test các endpoint:

```
https://localhost:3636/api/CategoryAPIs
https://localhost:3636/api/BrandAPIs
https://localhost:3636/api/ProductAPIs/latest/12
```

3. N?u th?y JSON data ? API ho?t ??ng t?t ?
4. N?u l?i 500 ho?c không có data ? Ki?m tra database

## ?? Troubleshooting

### L?i: "Cannot connect to SQL Server"
```bash
# Ki?m tra SQL Server ?ang ch?y
# Option 1: SQL Server Configuration Manager
# Option 2: Services (services.msc) ? SQL Server (MSSQLSERVER)

# Test connection string
dotnet ef database update --project TearGearAPI
```

### L?i: "CategoryBar ViewComponent not found"
- ? ?ã fix trong commit này
- ??m b?o file `CategoryBarViewComponent.cs` t?n t?i
- Build l?i solution: Ctrl + Shift + B

### L?i: "API returned 404"
- Ki?m tra TearGearAPI ?ã ch?y ch?a
- Ki?m tra port 3636 có b? chi?m d?ng không
- Ki?m tra firewall có block không

### L?i: "No data displayed on homepage"
```bash
# Ki?m tra có d? li?u trong database không
# N?u không có, c?n insert d? li?u m?u

# T?o migration n?u c?n
cd TearGearAPI
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Database tr?ng - Insert d? li?u m?u
```sql
-- Categories
INSERT INTO categoryAPIs (CategoryName, Description, IsActive, CreatedAt)
VALUES 
('Laptop Gaming', 'Laptop ch?i game hi?u n?ng cao', 1, GETDATE()),
('PC Gaming', 'Máy tính ch?i game', 1, GETDATE()),
('Linh ki?n PC', 'Linh ki?n nâng c?p PC', 1, GETDATE()),
('Bàn phím c?', 'Bàn phím c? gaming', 1, GETDATE()),
('Chu?t Gaming', 'Chu?t ch?i game', 1, GETDATE());

-- Brands
INSERT INTO brandAPIs (BrandName, Description, IsActive, LogoUrl)
VALUES 
('ASUS', 'ASUS ROG Gaming', 1, NULL),
('MSI', 'MSI Gaming', 1, NULL),
('Logitech', 'Logitech Gaming', 1, NULL),
('Razer', 'Razer Gaming', 1, NULL),
('Corsair', 'Corsair Gaming', 1, NULL);
```

## ?? URLs sau khi ch?y

- **API Swagger**: https://localhost:3636/swagger
- **MVC Frontend**: https://localhost:7002 (ho?c port ???c assign)
- **Homepage**: https://localhost:7002
- **Products**: https://localhost:7002/Products

## ?? Ki?m tra ho?t ??ng

1. ? Homepage hi?n th? 12 s?n ph?m m?i nh?t
2. ? Menu danh m?c hi?n th? ??y ?? categories
3. ? Click vào category ? Hi?n th? s?n ph?m theo category
4. ? Click vào s?n ph?m ? Hi?n th? chi ti?t
5. ? Không có l?i 500 Internal Server Error

## ?? Liên h?
N?u v?n g?p l?i, ki?m tra:
- Output window trong Visual Studio (View ? Output)
- Browser Console (F12)
- Log files n?u có

Good luck! ??
