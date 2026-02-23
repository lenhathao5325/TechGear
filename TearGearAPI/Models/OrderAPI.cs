using TechGearAPI.Models;
using static TechGearAPI.Models.Enums.EnumsAPI;

namespace TechGearAPI.Models
{
    public class OrderAPI
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
        public ApplicationUserAPI User { get; set; }
        public UserAddressAPI? UserAddress { get; set; }

        public ICollection<OrderDetailAPI> OrderDetails { get; set; }
            = new List<OrderDetailAPI>();

        public ICollection<PaymentAPI> Payments { get; set; }
            = new List<PaymentAPI>();
    }
}
