using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TearGearAPI.Model;
using TechGearAPI.Models;
using TechGearAPI.Models.Enums;

namespace TearGearAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUserAPI>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public DbSet<BrandAPI> brandAPIs { get; set; }
        public DbSet<ComboAPI> comboAPIs { get; set; }
        public DbSet<ComboItemAPI> comboItems { get; set; }
        public DbSet<ProductAPI> productAPIs { get; set; }
        public DbSet<ProductVariantAPI> productVariantAPIs { get; set; }
        public DbSet<CategoryAPI> categoryAPIs { get; set; }
        public DbSet<ProductImageAPI> productImageAPIs { get; set; }
        public DbSet<ProductOptionAPI> productOptionAPIs { get; set; }
        public DbSet<VariantOptionValueAPI> variantOptionValueAPIs { get; set; }
        public DbSet<ProductOptionValueAPI> productOptionValueAPIs { get; set; }
        public DbSet<CartItemAPI> cartItemAPIs { get; set; }
        public DbSet<OrderAPI> orderAPIs { get; set; }
        public DbSet<OrderDetailAPI> orderDetailAPIs { get; set; }
        public DbSet<ApplicationUserAPI> userAPIs { get; set; }
        public DbSet<UserAddressAPI> userAddressAPIs { get; set; }
        public DbSet<PaymentAPI> paymentAPIs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ---------- Primary Keys ----------
            builder.Entity<BrandAPI>()
                .HasKey(b => b.BrandId);

            builder.Entity<CategoryAPI>()
                .HasKey(c => c.CategoryId);

            builder.Entity<CartItemAPI>()
                .HasKey(c => c.CartItemId);

            builder.Entity<ComboAPI>()
                .HasKey(c => c.ComboId);

            builder.Entity<ComboItemAPI>()
                .HasKey(ci => ci.ComboItemId);

            builder.Entity<OrderAPI>()
                .HasKey(o => o.OrderId);

            builder.Entity<OrderDetailAPI>()
                .HasKey(od => od.OrderDetailId);

            builder.Entity<PaymentAPI>()
                .HasKey(p => p.PaymentId);

            builder.Entity<ProductVariantAPI>()
                .HasKey(pv => pv.Id);

            builder.Entity<UserAddressAPI>()
                .HasKey(ua => ua.UserAddressId);

            builder.Entity<ProductOptionValueAPI>()
                .HasKey(pov => pov.Id);

            builder.Entity<VariantOptionValueAPI>()
                .HasKey(vov => vov.Id);

            builder.Entity<ProductOptionAPI>()
                .HasKey(po => po.Id);

            builder.Entity<ProductImageAPI>()
                .HasKey(pi => pi.ProductImageId);

            // ---------- Decimal precision ----------
            builder.Entity<ProductVariantAPI>().Property(p => p.Price).HasPrecision(18, 2);
            builder.Entity<OrderAPI>().Property(o => o.SubTotal).HasPrecision(18, 2);
            builder.Entity<OrderAPI>().Property(o => o.ShippingFee).HasPrecision(18, 2);
            builder.Entity<OrderAPI>().Property(o => o.DiscountAmount).HasPrecision(18, 2);
            builder.Entity<OrderAPI>().Property(o => o.TotalAmount).HasPrecision(18, 2);
            builder.Entity<OrderDetailAPI>().Property(o => o.UnitPrice).HasPrecision(18, 2);
            builder.Entity<PaymentAPI>().Property(p => p.Amount).HasPrecision(18, 2);
            builder.Entity<ComboAPI>().Property(c => c.OriginalPrice).HasPrecision(18, 2);
            builder.Entity<ComboAPI>().Property(c => c.FinalPrice).HasPrecision(18, 2);
            builder.Entity<ComboAPI>().Property(c => c.DiscountValue).HasPrecision(18, 2);

            // ---------- UNIQUE ----------
            builder.Entity<ProductVariantAPI>()
                .HasIndex(v => v.SKU)
                .IsUnique();

            builder.Entity<CartItemAPI>()
                .HasIndex(c => new { c.UserId, c.ProductVariantId })
                .IsUnique();

            builder.Entity<VariantOptionValueAPI>()
                .HasIndex(x => new { x.ProductVariantId, x.ProductOptionValueId })
                .IsUnique();

            // ---------- Product ----------
            builder.Entity<ProductAPI>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ProductAPI>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- ProductOption ----------
            builder.Entity<ProductOptionAPI>()
                .HasOne(o => o.Product)
                .WithMany(p => p.ProductOptions)
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------- ProductOptionValue ----------
            builder.Entity<ProductOptionValueAPI>()
                .HasOne(v => v.ProductOption)
                .WithMany(o => o.ProductOptionValues)
                .HasForeignKey(v => v.ProductOptionId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------- ProductVariant ----------
            builder.Entity<ProductVariantAPI>()
                .HasOne(v => v.Product)
                .WithMany(p => p.ProductVariants)
                .HasForeignKey(v => v.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------- ProductVariantOption ----------
            builder.Entity<VariantOptionValueAPI>()
                .HasOne(vo => vo.ProductVariant)
                .WithMany(v => v.VariantOptionValues)
                .HasForeignKey(vo => vo.ProductVariantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<VariantOptionValueAPI>()
                .HasOne(vo => vo.ProductOptionValue)
                .WithMany(v => v.ProductVariantOptions)
                .HasForeignKey(vo => vo.ProductOptionValueId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- ProductImage ----------
            builder.Entity<ProductImageAPI>()
                .HasOne(i => i.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ProductImageAPI>()
                .HasOne(i => i.ProductVariant)
                .WithMany(v => v.ProductImages)
                .HasForeignKey(i => i.ProductVariantId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------- CartItem ----------
            builder.Entity<CartItemAPI>()
                .HasOne(c => c.User)
                .WithMany(u => u.CartItems)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CartItemAPI>()
                .HasOne(c => c.ProductVariant)
                .WithMany(v => v.CartItems)
                .HasForeignKey(c => c.ProductVariantId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------- UserAddress ----------
            builder.Entity<UserAddressAPI>()
                .HasOne(a => a.User)
                .WithMany(u => u.UserAddresses)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------- Order ----------
            builder.Entity<OrderAPI>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<OrderAPI>()
                .HasOne(o => o.UserAddress)
                .WithMany(a => a.Orders)
                .HasForeignKey(o => o.UserAddressId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- OrderDetail ----------
            builder.Entity<OrderDetailAPI>()
                .HasOne(d => d.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<OrderDetailAPI>()
                .HasOne(d => d.ProductVariant)
                .WithMany(v => v.OrderDetails)
                .HasForeignKey(d => d.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- Payment ----------
            builder.Entity<PaymentAPI>()
                .HasOne(p => p.Order)
                .WithMany(o => o.Payments)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------- Combo ----------
            builder.Entity<ComboItemAPI>()
                .HasOne(ci => ci.Combo)
                .WithMany(c => c.ComboItems)
                .HasForeignKey(ci => ci.ComboId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ComboItemAPI>()
                .HasOne(ci => ci.ProductVariant)
                .WithMany(v => v.ComboItems)
                .HasForeignKey(ci => ci.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);
            Seed(builder);
        }

        private void Seed(ModelBuilder builder)
        {
            var date = new DateTime(2026, 1, 1);
            var hasher = new PasswordHasher<ApplicationUserAPI>();

            // ================= USERS =================
            var users = new List<ApplicationUserAPI>();

            for (int i = 1; i <= 5; i++)
            {
                var user = new ApplicationUserAPI
                {
                    Id = $"user{i}",
                    UserName = $"demo{i}",
                    NormalizedUserName = $"DEMO{i}",
                    Email = $"demo{i}@gmail.com",
                    NormalizedEmail = $"DEMO{i}@GMAIL.COM",
                    EmailConfirmed = true,
                    FullName = $"Demo User {i}",
                    CreatedAt = date,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    IsDeleted = false
                };

                user.PasswordHash = hasher.HashPassword(user, "123456");
                users.Add(user);
            }

            builder.Entity<ApplicationUserAPI>().HasData(users);

            // ================= CATEGORY =================
            builder.Entity<CategoryAPI>().HasData(
                new CategoryAPI { CategoryId = 1, CategoryName = "Laptop", IsActive = true, CreatedAt = date },
                new CategoryAPI { CategoryId = 2, CategoryName = "Mouse", IsActive = true, CreatedAt = date },
                new CategoryAPI { CategoryId = 3, CategoryName = "Keyboard", IsActive = true, CreatedAt = date },
                new CategoryAPI { CategoryId = 4, CategoryName = "Monitor", IsActive = true, CreatedAt = date },
                new CategoryAPI { CategoryId = 5, CategoryName = "Headphone", IsActive = true, CreatedAt = date }
            );

            // ================= BRAND =================
            builder.Entity<BrandAPI>().HasData(
                new BrandAPI { BrandId = 1, BrandName = "ASUS", IsActive = true },
                new BrandAPI { BrandId = 2, BrandName = "Logitech", IsActive = true },
                new BrandAPI { BrandId = 3, BrandName = "Razer", IsActive = true },
                new BrandAPI { BrandId = 4, BrandName = "Dell", IsActive = true },
                new BrandAPI { BrandId = 5, BrandName = "Sony", IsActive = true }
            );

            // ================= PRODUCT =================
            builder.Entity<ProductAPI>().HasData(
                new ProductAPI { Id = 1, Name = "Laptop ASUS ROG", CategoryId = 1, BrandId = 1, ImageUrl = "laptop1.jpg" },
                new ProductAPI { Id = 2, Name = "Mouse Logitech G102", CategoryId = 2, BrandId = 2, ImageUrl = "mouse.jpg" },
                new ProductAPI { Id = 3, Name = "Keyboard Razer", CategoryId = 3, BrandId = 3, ImageUrl = "keyboard.jpg" },
                new ProductAPI { Id = 4, Name = "Monitor Dell 27\"", CategoryId = 4, BrandId = 4, ImageUrl = "monitor.jpg" },
                new ProductAPI { Id = 5, Name = "Sony Headphone", CategoryId = 5, BrandId = 5, ImageUrl = "headphone.jpg" }
            );

            // ================= VARIANT =================
            builder.Entity<ProductVariantAPI>().HasData(
                new ProductVariantAPI { Id = 1, ProductId = 1, Price = 20000000, Stock = 10, SKU = "SKU-001" },
                new ProductVariantAPI { Id = 2, ProductId = 2, Price = 500000, Stock = 50, SKU = "SKU-002" },
                new ProductVariantAPI { Id = 3, ProductId = 3, Price = 1500000, Stock = 30, SKU = "SKU-003" },
                new ProductVariantAPI { Id = 4, ProductId = 4, Price = 4000000, Stock = 20, SKU = "SKU-004" },
                new ProductVariantAPI { Id = 5, ProductId = 5, Price = 2500000, Stock = 40, SKU = "SKU-005" }
            );

            // ================= ADDRESS =================
            builder.Entity<UserAddressAPI>().HasData(
                new UserAddressAPI { UserAddressId = 1, UserId = "user1", FullName = "User 1", PhoneNumber = "111", AddressLine = "HCM", Province = "HCM", District = "Q1", Ward = "Ward 1", IsDefault = true },
                new UserAddressAPI { UserAddressId = 2, UserId = "user2", FullName = "User 2", PhoneNumber = "222", AddressLine = "HN", Province = "HN", District = "Ba Dinh", Ward = "Ward 2", IsDefault = true },
                new UserAddressAPI { UserAddressId = 3, UserId = "user3", FullName = "User 3", PhoneNumber = "333", AddressLine = "DN", Province = "DN", District = "Hai Chau", Ward = "Ward 3", IsDefault = true },
                new UserAddressAPI { UserAddressId = 4, UserId = "user4", FullName = "User 4", PhoneNumber = "444", AddressLine = "CT", Province = "CT", District = "Ninh Kieu", Ward = "Ward 4", IsDefault = true },
                new UserAddressAPI { UserAddressId = 5, UserId = "user5", FullName = "User 5", PhoneNumber = "555", AddressLine = "HP", Province = "HP", District = "Hong Bang", Ward = "Ward 5", IsDefault = true }
            );

            // ================= COMBO =================
            builder.Entity<ComboAPI>().HasData(
                new ComboAPI { ComboId = 1, ComboName = "Gaming Set 1", OriginalPrice = 22000000, FinalPrice = 21000000, DiscountType = EnumsAPI.DiscountType.Percentage, DiscountValue = 5, IsActive = true, CreatedAt = date },
                new ComboAPI { ComboId = 2, ComboName = "Gaming Set 2", OriginalPrice = 3000000, FinalPrice = 2700000, DiscountType = EnumsAPI.DiscountType.Percentage, DiscountValue = 10, IsActive = true, CreatedAt = date },
                new ComboAPI { ComboId = 3, ComboName = "Office Set", OriginalPrice = 6000000, FinalPrice = 5500000, DiscountType = EnumsAPI.DiscountType.FixedAmount, DiscountValue = 500000, IsActive = true, CreatedAt = date },
                new ComboAPI { ComboId = 4, ComboName = "Streamer Set", OriginalPrice = 7000000, FinalPrice = 6500000, DiscountType = EnumsAPI.DiscountType.FixedAmount, DiscountValue = 500000, IsActive = true, CreatedAt = date },
                new ComboAPI { ComboId = 5, ComboName = "Audio Set", OriginalPrice = 3000000, FinalPrice = 2800000, DiscountType = EnumsAPI.DiscountType.Percentage, DiscountValue = 7, IsActive = true, CreatedAt = date }
            );

            // ================= COMBO ITEM =================
            builder.Entity<ComboItemAPI>().HasData(
                new ComboItemAPI { ComboItemId = 1, ComboId = 1, ProductVariantId = 1, Quantity = 1 },
                new ComboItemAPI { ComboItemId = 2, ComboId = 2, ProductVariantId = 2, Quantity = 1 },
                new ComboItemAPI { ComboItemId = 3, ComboId = 3, ProductVariantId = 3, Quantity = 1 },
                new ComboItemAPI { ComboItemId = 4, ComboId = 4, ProductVariantId = 4, Quantity = 1 },
                new ComboItemAPI { ComboItemId = 5, ComboId = 5, ProductVariantId = 5, Quantity = 1 }
            );

            // ================= CART =================
            builder.Entity<CartItemAPI>().HasData(
                new CartItemAPI { CartItemId = 1, UserId = "user1", ProductVariantId = 1, Quantity = 1 },
                new CartItemAPI { CartItemId = 2, UserId = "user2", ProductVariantId = 2, Quantity = 2 },
                new CartItemAPI { CartItemId = 3, UserId = "user3", ProductVariantId = 3, Quantity = 1 },
                new CartItemAPI { CartItemId = 4, UserId = "user4", ProductVariantId = 4, Quantity = 1 },
                new CartItemAPI { CartItemId = 5, UserId = "user5", ProductVariantId = 5, Quantity = 3 }
            );

            // ================= ORDER =================
            builder.Entity<OrderAPI>().HasData(
                new OrderAPI { OrderId = 1, UserId = "user1", UserAddressId = 1, SubTotal = 20000000, ShippingFee = 50000, DiscountAmount = 0, TotalAmount = 20050000, Status = EnumsAPI.OrderStatus.Pending, OrderDate = date, CreatedAt = date },
                new OrderAPI { OrderId = 2, UserId = "user2", UserAddressId = 2, SubTotal = 500000, ShippingFee = 30000, DiscountAmount = 0, TotalAmount = 530000, Status = EnumsAPI.OrderStatus.Pending, OrderDate = date, CreatedAt = date },
                new OrderAPI { OrderId = 3, UserId = "user3", UserAddressId = 3, SubTotal = 1500000, ShippingFee = 30000, DiscountAmount = 0, TotalAmount = 1530000, Status = EnumsAPI.OrderStatus.Pending, OrderDate = date, CreatedAt = date },
                new OrderAPI { OrderId = 4, UserId = "user4", UserAddressId = 4, SubTotal = 4000000, ShippingFee = 50000, DiscountAmount = 0, TotalAmount = 4050000, Status = EnumsAPI.OrderStatus.Pending, OrderDate = date, CreatedAt = date },
                new OrderAPI { OrderId = 5, UserId = "user5", UserAddressId = 5, SubTotal = 2500000, ShippingFee = 50000, DiscountAmount = 0, TotalAmount = 2550000, Status = EnumsAPI.OrderStatus.Pending, OrderDate = date, CreatedAt = date }
            );

            // ================= ORDER DETAIL =================
            builder.Entity<OrderDetailAPI>().HasData(
                new OrderDetailAPI { OrderDetailId = 1, OrderId = 1, ProductVariantId = 1, ProductName = "Laptop ASUS ROG", VariantDescription = "Default", Quantity = 1, UnitPrice = 20000000 },
                new OrderDetailAPI { OrderDetailId = 2, OrderId = 2, ProductVariantId = 2, ProductName = "Mouse Logitech", VariantDescription = "Default", Quantity = 1, UnitPrice = 500000 },
                new OrderDetailAPI { OrderDetailId = 3, OrderId = 3, ProductVariantId = 3, ProductName = "Keyboard Razer", VariantDescription = "Default", Quantity = 1, UnitPrice = 1500000 },
                new OrderDetailAPI { OrderDetailId = 4, OrderId = 4, ProductVariantId = 4, ProductName = "Monitor Dell", VariantDescription = "Default", Quantity = 1, UnitPrice = 4000000 },
                new OrderDetailAPI { OrderDetailId = 5, OrderId = 5, ProductVariantId = 5, ProductName = "Sony Headphone", VariantDescription = "Default", Quantity = 1, UnitPrice = 2500000 }
            );

            // ================= PAYMENT =================
            builder.Entity<PaymentAPI>().HasData(
                new PaymentAPI { PaymentId = 1, OrderId = 1, Amount = 20050000, Method = (int)EnumsAPI.PaymentMethod.COD, Status = (int)EnumsAPI.PaymentStatus.Pending, TransactionId = "TRANS001", PaymentDate = date },
                new PaymentAPI { PaymentId = 2, OrderId = 2, Amount = 530000, Method = (int)EnumsAPI.PaymentMethod.COD, Status = (int)EnumsAPI.PaymentStatus.Pending, TransactionId = "TRANS002", PaymentDate = date },
                new PaymentAPI { PaymentId = 3, OrderId = 3, Amount = 1530000, Method = (int)EnumsAPI.PaymentMethod.COD, Status = (int)EnumsAPI.PaymentStatus.Pending, TransactionId = "TRANS003", PaymentDate = date },
                new PaymentAPI { PaymentId = 4, OrderId = 4, Amount = 4050000, Method = (int)EnumsAPI.PaymentMethod.COD, Status = (int)EnumsAPI.PaymentStatus.Pending, TransactionId = "TRANS004", PaymentDate = date },
                new PaymentAPI { PaymentId = 5, OrderId = 5, Amount = 2550000, Method = (int)EnumsAPI.PaymentMethod.COD, Status = (int)EnumsAPI.PaymentStatus.Pending, TransactionId = "TRANS005", PaymentDate = date }
            );
        }


    }
}
