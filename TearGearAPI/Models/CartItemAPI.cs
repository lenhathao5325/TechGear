namespace TechGearAPI.Models
{

        public class CartItemAPI
        {
            public int CartItemId { get; set; }

            public string UserId { get; set; }
            public int ProductVariantId { get; set; }

            public int Quantity { get; set; }

            public ApplicationUserAPI User { get; set; }
            public ProductVariantAPI ProductVariant { get; set; }
        }

    
}