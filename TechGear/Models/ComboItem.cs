using TechGear.Models;

namespace TechGear.Models
{
    /// <summary>
    /// ViewModel for displaying Combo Item in MVC views
    /// Data received from API
    /// </summary>
    public class ComboItem
    {
        public int ComboItemId { get; set; }
        public int ComboId { get; set; }
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
        
        // Flattened properties for display
        public string? ComboName { get; set; }
        public string? ProductName { get; set; }
        public string? VariantDescription { get; set; }
        public string? ProductImageUrl { get; set; }
        public decimal? UnitPrice { get; set; }
        
        // Calculated for display
        public decimal? TotalPrice => Quantity * (UnitPrice ?? 0);
    }
}