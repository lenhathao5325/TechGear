using TechGear.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechGear.Models;

namespace TechGear.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // ===================== DbSet =====================
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductOption> ProductOptions { get; set; }
        public DbSet<ProductOptionValue> ProductOptionValues { get; set; }
        public DbSet<ProductVariant> ProductVariants { get; set; }
        public DbSet<VariantOptionValue> ProductVariantOptions { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }

        public DbSet<CartItem> CartItems { get; set; }

        public DbSet<UserAddress> UserAddresses { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Payment> Payments { get; set; }

        public DbSet<Combo> Combos { get; set; }
        public DbSet<ComboItem> ComboItems { get; set; }


        // ===================== Fluent API =====================
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // ---------- Decimal precision ----------
            builder.Entity<ProductVariant>().Property(p => p.Price).HasPrecision(18, 2);
            builder.Entity<Order>().Property(o => o.SubTotal).HasPrecision(18, 2);
            builder.Entity<Order>().Property(o => o.ShippingFee).HasPrecision(18, 2);
            builder.Entity<Order>().Property(o => o.DiscountAmount).HasPrecision(18, 2);
            builder.Entity<Order>().Property(o => o.TotalAmount).HasPrecision(18, 2);
            builder.Entity<OrderDetail>().Property(o => o.UnitPrice).HasPrecision(18, 2);
            builder.Entity<Payment>().Property(p => p.Amount).HasPrecision(18, 2);
            builder.Entity<Combo>().Property(c => c.OriginalPrice).HasPrecision(18, 2);
            builder.Entity<Combo>().Property(c => c.FinalPrice).HasPrecision(18, 2);
            builder.Entity<Combo>().Property(c => c.DiscountValue).HasPrecision(18, 2);

            // ---------- UNIQUE ----------
            builder.Entity<ProductVariant>()
                .HasIndex(v => v.SKU)
                .IsUnique();

            builder.Entity<CartItem>()
                .HasIndex(c => new { c.UserId, c.ProductVariantId })
                .IsUnique();

            builder.Entity<VariantOptionValue>()
                .HasIndex(x => new { x.ProductVariantId, x.ProductOptionValueId })
                .IsUnique();

            // ---------- Product ----------
            builder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Product>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- ProductOption ----------
            builder.Entity<ProductOption>()
                .HasOne(o => o.Product)
                .WithMany(p => p.ProductOptions)
                .HasForeignKey(o => o.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------- ProductOptionValue ----------
            builder.Entity<ProductOptionValue>()
                .HasOne(v => v.ProductOption)
                .WithMany(o => o.ProductOptionValues)
                .HasForeignKey(v => v.ProductOptionId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------- ProductVariant ----------
            builder.Entity<ProductVariant>()
                .HasOne(v => v.Product)
                .WithMany(p => p.ProductVariants)
                .HasForeignKey(v => v.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------- ProductVariantOption ----------
            builder.Entity<VariantOptionValue>()
                .HasOne(vo => vo.ProductVariant)
                .WithMany(v => v.VariantOptionValues)
                .HasForeignKey(vo => vo.ProductVariantId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<VariantOptionValue>()
                .HasOne(vo => vo.ProductOptionValue)
                .WithMany(v => v.ProductVariantOptions)
                .HasForeignKey(vo => vo.ProductOptionValueId)
                .OnDelete(DeleteBehavior.Restrict); // 🔥 FIX CASCADE

            // ---------- ProductImage ----------
            builder.Entity<ProductImage>()
                .HasOne(i => i.Product)
                .WithMany(p => p.Images)
                .HasForeignKey(i => i.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ProductImage>()
                .HasOne(i => i.ProductVariant)
                .WithMany(v => v.ProductImages)
                .HasForeignKey(i => i.ProductVariantId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------- CartItem ----------
            builder.Entity<CartItem>()
                .HasOne(c => c.User)
                .WithMany(u => u.CartItems)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CartItem>()
                .HasOne(c => c.ProductVariant)
                .WithMany(v => v.CartItems)
                .HasForeignKey(c => c.ProductVariantId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------- UserAddress ----------
            builder.Entity<UserAddress>()
                .HasOne(a => a.User)
                .WithMany(u => u.UserAddresses)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------- Order (FIX MULTIPLE CASCADE) ----------
            builder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Order>()
                .HasOne(o => o.UserAddress)
                .WithMany(a => a.Orders)
                .HasForeignKey(o => o.UserAddressId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- OrderDetail ----------
            builder.Entity<OrderDetail>()
                .HasOne(d => d.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<OrderDetail>()
                .HasOne(d => d.ProductVariant)
                .WithMany(v => v.OrderDetails)
                .HasForeignKey(d => d.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);

            // ---------- Payment ----------
            builder.Entity<Payment>()
                .HasOne(p => p.Order)
                .WithMany(o => o.Payments)
                .HasForeignKey(p => p.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // ---------- Combo ----------
            builder.Entity<ComboItem>()
                .HasOne(ci => ci.Combo)
                .WithMany(c => c.ComboItems)
                .HasForeignKey(ci => ci.ComboId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ComboItem>()
                .HasOne(ci => ci.ProductVariant)
                .WithMany(v => v.ComboItems)
                .HasForeignKey(ci => ci.ProductVariantId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
