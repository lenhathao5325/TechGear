using TechGear.Models;

namespace TechGear.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }
        public int ProductVariantId { get; set; }

        public string ProductName { get; set; }
        public string VariantDescription { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

        // Navigation
        public Order Order { get; set; }
        public ProductVariant ProductVariant { get; set; }
    }

}
