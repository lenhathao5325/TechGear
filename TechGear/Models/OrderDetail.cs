using TechGear.Models;

namespace TechGear.Models
{
    /// <summary>
    /// ViewModel for displaying Order Detail in MVC views
    /// Data received from API
    /// </summary>
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductVariantId { get; set; }

        // Product information (flattened from API)
        public string ProductName { get; set; }
        public string VariantDescription { get; set; }
        public string? ProductImageUrl { get; set; }

        // Order details
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        
        // Calculated for display
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
