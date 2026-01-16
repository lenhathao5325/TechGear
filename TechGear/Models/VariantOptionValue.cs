namespace TechGear.Models
{
    public class VariantOptionValue
    {
        public int Id { get; set; }   // Primary Key

        public int ProductVariantId { get; set; }
        public int ProductOptionValueId { get; set; }

        // Navigation
        public ProductVariant ProductVariant { get; set; }
        public ProductOptionValue ProductOptionValue { get; set; }
    }
}
