namespace TechGearAPI.Models
{
    public class VariantOptionValueAPI
    {
        public int Id { get; set; }   // Primary Key

        public int ProductVariantId { get; set; }
        public int ProductOptionValueId { get; set; }

        // Navigation
        public ProductVariantAPI ProductVariant { get; set; }
        public ProductOptionValueAPI ProductOptionValue { get; set; }
    }
}
