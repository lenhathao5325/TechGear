using TechGear.Models;

using Microsoft.AspNetCore.Identity;  
namespace TechGear.Models
{
    /// <summary>
    /// ViewModel for displaying Cart Item in MVC views
    /// Data received from API
    /// </summary>
    public class CartItem
    {
        public int CartItemId { get; set; }
        public string UserId { get; set; }
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }

        // Flattened properties from ProductVariant and Product
        public string? ProductName { get; set; }
        public string? VariantDescription { get; set; }
        public string? ProductImageUrl { get; set; }
        public decimal UnitPrice { get; set; }
        public int AvailableStock { get; set; }
        
        // Calculated for display
        public decimal TotalPrice => Quantity * UnitPrice;
    }
}
