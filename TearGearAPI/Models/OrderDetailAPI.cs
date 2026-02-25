using TechGearAPI.Models;

namespace TechGearAPI.Models
{
    public class OrderDetailAPI
    {
        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }
        public int ProductVariantId { get; set; }

        public string ProductName { get; set; }
        public string VariantDescription { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // Navigation
        public OrderAPI Order { get; set; }
        public ProductVariantAPI ProductVariant { get; set; }
    }

}
