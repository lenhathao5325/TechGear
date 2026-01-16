using TechGear.Models;

namespace TechGear.Models
{
    public class ComboItem
    {
        public int ComboItemId { get; set; }

        public int ComboId { get; set; }
        public int ProductVariantId { get; set; }

        public int Quantity { get; set; }

        // Navigation
        public Combo Combo { get; set; } = null!;
        public ProductVariant ProductVariant { get; set; } = null!;
    }
}