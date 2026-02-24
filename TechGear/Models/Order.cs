using static TechGear.Models.Enums.Enums;

namespace TechGear.Models
{
    /// <summary>
    /// ViewModel for displaying Order information in MVC views
    /// Data received from API
    /// </summary>
    public class Order
    {
        public int OrderId { get; set; }

        // User Information (flattened)
        public string UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserEmail { get; set; }
        
        // Address Information (flattened)
        public int? UserAddressId { get; set; }
        public string? ShippingAddress { get; set; } // Full formatted address

        // Money
        public decimal SubTotal { get; set; }
        public decimal ShippingFee { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TotalAmount { get; set; }

        // Status
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public string? StatusText { get; set; } // For display: "Chờ xử lý", "Đã xác nhận", etc.

        // Time
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Related data for display
        public List<OrderDetail>? OrderDetails { get; set; }
        public List<Payment>? Payments { get; set; }
    }
}
