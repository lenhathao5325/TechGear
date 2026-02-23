using TechGearAPI.Models;

namespace TechGearAPI.Models
{
    public class ComboItemAPI
    {
        public int ComboItemId { get; set; }

        public int ComboId { get; set; }
        public int ProductVariantId { get; set; }

        public int Quantity { get; set; }

        // Navigation
        public ComboAPI Combo { get; set; } = null!;
        public ProductVariantAPI ProductVariant { get; set; } = null!;
    }
}