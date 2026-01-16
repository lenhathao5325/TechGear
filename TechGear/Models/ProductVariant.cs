using TechGear.Models;

namespace TechGear.Models
{
    public class ProductVariant
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string SKU { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        // Navigation
        public Product Product { get; set; }
        public ICollection<VariantOptionValue> ProductVariantOptions { get; set; } = new List<VariantOptionValue>();
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

        public ICollection<VariantOptionValue> VariantOptionValues { get; set; } = new List<VariantOptionValue>();

        public ICollection<ComboItem> ComboItems { get; set; } = new List<ComboItem>();
        public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    }
}
