using TechGearAPI.Models;

namespace TechGearAPI.Models
{
    public class ProductOptionValueAPI
    {
        public int Id { get; set; }

        public int ProductOptionId { get; set; }
        public string Value { get; set; }

        // Navigation
        public ProductOptionAPI ProductOption { get; set; }
        public ICollection<VariantOptionValueAPI> ProductVariantOptions { get; set; } = new List<VariantOptionValueAPI>();
    }
}
