using TechGear.Models;

using Microsoft.AspNetCore.Identity;  
namespace TechGear.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }

        public string UserId { get; set; }
        public int ProductVariantId { get; set; }

        public int Quantity { get; set; }

        // Navigation
        public ApplicationUser User { get; set; }
        public ProductVariant ProductVariant { get; set; }
    }
}
