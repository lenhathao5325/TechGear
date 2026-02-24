using static TechGear.Models.Enums.Enums;

namespace TechGear.Models
{
    /// <summary>
    /// ViewModel for displaying Payment information in MVC views
    /// Data received from API
    /// </summary>
    public class Payment
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        
        // Method: 1: Tiền mặt | 2: Chuyển khoản | 3: COD
        public int Method { get; set; }
        public string? MethodText { get; set; } // For display: "Tiền mặt", "Chuyển khoản", "COD"
        
        // Status: 1: Pending | 2: Paid | 3: Failed
        public PaymentStatus Status { get; set; }
        public string? StatusText { get; set; } // For display: "Chờ thanh toán", "Đã thanh toán", "Thất bại"
        
        public string? TransactionId { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}
