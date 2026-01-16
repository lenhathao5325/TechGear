using TechGear.Models;
using static TechGear.Models.Enums.Enums;

namespace TechGear.Models
{
    public class Order
    {
        public int OrderId { get; set; }

        // ====== FK ======
        public string UserId { get; set; }
        public int? UserAddressId { get; set; }

        // ====== Money ======
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }

        // ====== Status ======
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        // ====== Time ======
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // ====== Navigation ======
        public ApplicationUser User { get; set; }
        public UserAddress? UserAddress { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
            = new List<OrderDetail>();

        public ICollection<Payment> Payments { get; set; }
            = new List<Payment>();
    }
}
