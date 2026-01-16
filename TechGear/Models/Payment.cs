namespace TechGear.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }

        public int OrderId { get; set; }
        public decimal Amount { get; set; }

        // 1: Tiền mặt | 2: Chuyển khoản | 3: COD
        public int Method { get; set; }

        // 1: Chưa thanh toán | 2: Đã thanh toán | 3: Hoàn tiền
        public int Status { get; set; }

        public string? TransactionId { get; set; }
        public DateTime? PaymentDate { get; set; }

        // Navigation
        public Order Order { get; set; }
    }
}
