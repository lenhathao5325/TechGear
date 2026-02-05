using TechGearAPI.Models;

namespace TechGearAPI.Models
{
    public class ProductVariantAPI
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string SKU { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        // Navigation
        public ProductAPI? Product { get; set; }
        public ICollection<VariantOptionValueAPI> ProductVariantOptions { get; set; } = new List<VariantOptionValueAPI>();
        public ICollection<CartItemAPI> CartItems { get; set; } = new List<CartItemAPI>();

        public ICollection<OrderDetailAPI> OrderDetails { get; set; } = new List<OrderDetailAPI>();

        public ICollection<VariantOptionValueAPI> VariantOptionValues { get; set; } = new List<VariantOptionValueAPI>();

        public ICollection<ComboItemAPI> ComboItems { get; set; } = new List<ComboItemAPI>();
        public ICollection<ProductImageAPI> ProductImages { get; set; } = new List<ProductImageAPI>();
        public int ProductVariantId { get; internal set; }
    }
}
