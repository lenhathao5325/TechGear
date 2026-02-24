namespace TechGear.Models
{
    /// <summary>
    /// ViewModel for displaying Variant Option Value in MVC views
    /// Data received from API
    /// </summary>
    public class VariantOptionValue
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public int ProductOptionValueId { get; set; }
        
        // For display purposes (flattened)
        public string? OptionName { get; set; } // e.g., "Color"
        public string? OptionValue { get; set; } // e.g., "Red"
    }
}
