using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechGearAPI.Models;

namespace TechGearAPI.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUserAPI>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
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
        }
    } 
}
