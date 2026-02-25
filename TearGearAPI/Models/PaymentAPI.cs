namespace TechGearAPI.Models
{
    public class PaymentAPI
    {
        public int PaymentId { get; set; }

        public int OrderId { get; set; }
        public decimal Amount { get; set; }

        // 1: Ti?n m?t | 2: Chuy?n kho?n | 3: COD
        public int Method { get; set; }

        // 1: Chua thanh toán | 2: Ðã thanh toán | 3: Hoàn ti?n
        public int Status { get; set; }

        public string? TransactionId { get; set; }
        public DateTime? PaymentDate { get; set; }

        // Navigation
        public OrderAPI Order { get; set; }
    }
}
