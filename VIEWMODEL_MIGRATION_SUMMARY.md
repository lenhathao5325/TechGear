# T?NG K?T: CHUY?N ??I MODELS SANG VIEWMODELS

## ?? M?C ?ÍCH
Tách bi?t rõ ràng gi?a:
- **API (TearGearAPI)**: S? d?ng models g?c (Entity Framework) + DTOs
- **MVC (TechGear)**: S? d?ng ViewModels ??n gi?n ?? hi?n th? UI

## ? CÁC MODELS ?Ã CHUY?N ??I

### 1. Brand.cs
**Tr??c:**
```csharp
public List<Product> Products { get; set; } // Navigation property
```

**Sau:**
```csharp
public int? ProductCount { get; set; } // For display only
```

---

### 2. Category.cs
**Tr??c:**
```csharp
public ICollection<Product> Products { get; set; } = new List<Product>();
```

**Sau:**
```csharp
public int? ProductCount { get; set; } // For display only
```

---

### 3. Product.cs
**Tr??c:**
```csharp
public Category Category { get; set; }
public Brand Brand { get; set; }
public ICollection<ProductImage> Images { get; set; }
public ICollection<ProductOption> ProductOptions { get; set; }
public ICollection<ProductVariant> ProductVariants { get; set; }
```

**Sau:**
```csharp
// Flattened properties
public string? CategoryName { get; set; }
public string? BrandName { get; set; }

// Simple lists for display
public List<string>? ImageUrls { get; set; }
public List<ProductVariant>? Variants { get; set; }

// Calculated properties
public decimal? MinPrice { get; set; }
public decimal? MaxPrice { get; set; }
public int? TotalStock { get; set; }
```

---

### 4. ProductVariant.cs
**Tr??c:**
```csharp
public Product Product { get; set; }
public ICollection<VariantOptionValue> ProductVariantOptions { get; set; }
public ICollection<CartItem> CartItems { get; set; }
public ICollection<OrderDetail> OrderDetails { get; set; }
public ICollection<ComboItem> ComboItems { get; set; }
public ICollection<ProductImage> ProductImages { get; set; }
```

**Sau:**
```csharp
// Flattened
public string? ProductName { get; set; }
public string? VariantDescription { get; set; }

// For display
public List<string>? ImageUrls { get; set; }
public Dictionary<string, string>? Options { get; set; } // {"Color": "Red", "Size": "XL"}
```

---

### 5. Order.cs
**Tr??c:**
```csharp
public User User { get; set; }
public UserAddress? UserAddress { get; set; }
public List<OrderDetail> OrderDetails { get; set; }
public List<Payment> Payments { get; set; }
```

**Sau:**
```csharp
// Flattened user info
public string? UserName { get; set; }
public string? UserEmail { get; set; }

// Flattened address
public string? ShippingAddress { get; set; }

// Status text for display
public string? StatusText { get; set; }

// Simple lists (not navigation properties)
public List<OrderDetail>? OrderDetails { get; set; }
public List<Payment>? Payments { get; set; }
```

---

### 6. OrderDetail.cs
**Tr??c:**
```csharp
public Order Order { get; set; }
public ProductVariant ProductVariant { get; set; }
```

**Sau:**
```csharp
// Flattened
public string ProductName { get; set; }
public string VariantDescription { get; set; }
public string? ProductImageUrl { get; set; }

// Calculated
public decimal TotalPrice => Quantity * UnitPrice;
```

---

### 7. Payment.cs
**Tr??c:**
```csharp
public Order Order { get; set; }
```

**Sau:**
```csharp
// Text properties for display
public string? MethodText { get; set; } // "Ti?n m?t", "Chuy?n kho?n", "COD"
public string? StatusText { get; set; } // "Ch? thanh toán", "?ã thanh toán"
```

---

### 8. CartItem.cs
**Tr??c:**
```csharp
public User User { get; set; }
public ProductVariant ProductVariant { get; set; }
```

**Sau:**
```csharp
// Flattened
public string? ProductName { get; set; }
public string? VariantDescription { get; set; }
public string? ProductImageUrl { get; set; }
public decimal UnitPrice { get; set; }
public int AvailableStock { get; set; }

// Calculated
public decimal TotalPrice => Quantity * UnitPrice;
```

---

### 9. UserAddress.cs
**Tr??c:**
```csharp
public User User { get; set; }
public ICollection<Order> Orders { get; set; }
```

**Sau:**
```csharp
// Calculated
public string FullAddress => $"{AddressLine}, {Ward}, {District}, {Province}";
```

---

### 10. User.cs
**Tr??c:**
```csharp
public int UserId { get; set; }
public List<UserAddress> UserAddresses { get; set; }
```

**Sau:**
```csharp
public string UserId { get; set; } // Changed to string (match Identity)
public List<UserAddress>? Addresses { get; set; }
public int? TotalOrders { get; set; }
```

---

### 11. ProductImage.cs
**Tr??c:**
```csharp
public Product Product { get; set; }
public ProductVariant ProductVariant { get; set; }
```

**Sau:**
```csharp
public string? ProductName { get; set; }
public string? VariantDescription { get; set; }
```

---

### 12. ProductOption.cs
**Tr??c:**
```csharp
public Product Product { get; set; }
public ICollection<ProductOptionValue> ProductOptionValues { get; set; }
```

**Sau:**
```csharp
public string? ProductName { get; set; }
public List<ProductOptionValue>? Values { get; set; }
```

---

### 13. ProductOptionValue.cs
**Tr??c:**
```csharp
public ProductOption ProductOption { get; set; }
public ICollection<VariantOptionValue> ProductVariantOptions { get; set; }
```

**Sau:**
```csharp
public string? OptionName { get; set; }
```

---

### 14. VariantOptionValue.cs
**Tr??c:**
```csharp
public ProductVariant ProductVariant { get; set; }
public ProductOptionValue ProductOptionValue { get; set; }
```

**Sau:**
```csharp
public string? OptionName { get; set; }
public string? OptionValue { get; set; }
```

---

### 15. Combo.cs
**Tr??c:**
```csharp
public ICollection<ComboItem> ComboItems { get; set; }
```

**Sau:**
```csharp
public List<ComboItem>? Items { get; set; }
public string? DiscountText { get; set; }
public decimal SavingsAmount => OriginalPrice - FinalPrice;
```

---

### 16. ComboItem.cs
**Tr??c:**
```csharp
public Combo Combo { get; set; }
public ProductVariant ProductVariant { get; set; }
```

**Sau:**
```csharp
// Flattened
public string? ComboName { get; set; }
public string? ProductName { get; set; }
public string? VariantDescription { get; set; }
public string? ProductImageUrl { get; set; }
public decimal? UnitPrice { get; set; }

// Calculated
public decimal? TotalPrice => Quantity * (UnitPrice ?? 0);
```

---

## ?? NGUYÊN T?C CHÍNH

### ? DO (Làm)
1. **Flattened Properties**: Thay navigation properties b?ng các property ??n gi?n
   - `Product.Brand` ? `Product.BrandName`
   - `Order.User` ? `Order.UserName`, `Order.UserEmail`

2. **Text Properties**: Thêm text properties cho display
   - `Payment.Method` (int) ? thêm `Payment.MethodText` (string)
   - `Order.Status` (enum) ? thêm `Order.StatusText` (string)

3. **Calculated Properties**: Thêm computed properties
   - `OrderDetail.TotalPrice => Quantity * UnitPrice`
   - `Combo.SavingsAmount => OriginalPrice - FinalPrice`

4. **Simple Lists**: Dùng List<T> thay vì ICollection<T>
   - `List<OrderDetail>` thay vì `ICollection<OrderDetail>`

### ? DON'T (Không làm)
1. **Không dùng Navigation Properties**
   - ? `public Brand Brand { get; set; }`
   - ? `public string? BrandName { get; set; }`

2. **Không dùng ICollection v?i navigation**
   - ? `public ICollection<Product> Products { get; set; }`
   - ? `public int? ProductCount { get; set; }`

3. **Không dùng Entity Framework attributes**
   - ? `[ForeignKey]`, `[InverseProperty]`
   - ? Ch? dùng validation attributes: `[Required]`, `[Display]`

---

## ?? WORKFLOW: API ? MVC

```
???????????????????????????????
?    TearGearAPI (Backend)    ?
???????????????????????????????
? 1. Entity Models            ?
?    - ProductAPI             ?
?    - BrandAPI               ?
?    - With navigation props  ?
?                             ?
? 2. DTOs                     ?
?    - ProductDTO             ?
?    - CreateProductDTO       ?
?                             ?
? 3. API Response             ?
?    {                        ?
?      "id": 1,               ?
?      "name": "Laptop",      ?
?      "brandId": 5,          ?
?      "brand": {             ?
?        "brandId": 5,        ?
?        "brandName": "Dell"  ?
?      }                      ?
?    }                        ?
???????????????????????????????
           ? HTTP/JSON
???????????????????????????????
?    TechGear MVC (Frontend)  ?
???????????????????????????????
? 1. API Service Layer        ?
?    - Deserialize JSON       ?
?    - Map to ViewModel       ?
?                             ?
? 2. ViewModels (Flattened)   ?
?    public class Product     ?
?    {                        ?
?      public int Id          ?
?      public string Name     ?
?      public int? BrandId    ?
?      public string BrandName? // ? Flattened
?    }                        ?
?                             ?
? 3. Controller               ?
?    var products = await     ?
?      _apiService            ?
?      .GetAsync<List<Product>>?
?                             ?
? 4. View                     ?
?    @item.BrandName          ? // ? Direct access
???????????????????????????????
```

---

## ?? THAY ??I VIEWS C?N THI?T

### Tr??c (Sai):
```razor
@item.Brand?.BrandName
@item.Category?.CategoryName
@item.ProductVariants.Count
```

### Sau (?úng):
```razor
@item.BrandName
@item.CategoryName
@item.Variants?.Count
```

---

## ? CÁC FILE KHÔNG S?A

1. **CloudinarySettings.cs** - Configuration class, không ph?i data model
2. **ErrorViewModel.cs** - Local error handling model
3. **ViewModels/ProductVM.cs** - Form model cho Create/Edit
4. **ViewModels/BrandVM.cs** - Form model
5. **ViewModels/RoleVM.cs** - Identity management
6. **ViewModels/UserRoleVM.cs** - Identity management
7. **Enums.cs** - Enum definitions

---

## ?? B??C TI?P THEO

1. ? Fix các Views ?? dùng flattened properties
2. ? T?o API Service Layer trong MVC
3. ? T?o Controllers trong MVC
4. ? Configure HttpClient trong Program.cs
5. ? Test k?t n?i API ? MVC

---

## ?? GHI CHÚ

- T?t c? models trong `TechGear/Models/` gi? là **ViewModels**
- Chúng ch? ch?a data ?? hi?n th?, không có logic database
- API s? tr? v? data ?ã flatten, MVC ch? c?n map tr?c ti?p
- Không c?n Entity Framework trong MVC project

