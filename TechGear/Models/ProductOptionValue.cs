using TechGear.Models;

namespace TechGear.Models
{
    /// <summary>
    /// ViewModel for displaying Product Option Value in MVC views
    /// Data received from API
    /// </summary>
    public class ProductOptionValue
    {
        public int Id { get; set; }
        public int ProductOptionId { get; set; }
        public string Value { get; set; } // e.g., "Red", "XL", "128GB"
        
        // For display purposes
        public string? OptionName { get; set; } // e.g., "Color", "Size"
    }
}
