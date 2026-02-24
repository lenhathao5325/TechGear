using TechGear.Models;

namespace TechGear.Models
{
    /// <summary>
    /// ViewModel for displaying Product Variant information in MVC views
    /// Data received from API
    /// </summary>
    public class ProductVariant
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        
        // Flattened properties for display
        public string? ProductName { get; set; }
        public string? VariantDescription { get; set; } // e.g., "Color: Red, Size: XL"
        
        // For display purposes
        public List<string>? ImageUrls { get; set; }
        public Dictionary<string, string>? Options { get; set; } // e.g., {"Color": "Red", "Size": "XL"}
    }
}
