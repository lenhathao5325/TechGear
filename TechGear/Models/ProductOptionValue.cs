using TechGear.Models;

namespace TechGear.Models
{
    public class ProductOptionValue
    {
        public int Id { get; set; }

        public int ProductOptionId { get; set; }
        public string Value { get; set; }

        // Navigation
        public ProductOption ProductOption { get; set; }
        public ICollection<VariantOptionValue> ProductVariantOptions { get; set; } = new List<VariantOptionValue>();
    }
}
