namespace TearGearAPI.DTO
{
    public class PaymentDTO
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }

        // 1: Ti?n m?t | 2: Chuy?n kho?n | 3: COD
        public int Method { get; set; }

        // 1: Ch?a thanh toán | 2: ?ã thanh toán | 3: Hoàn ti?n
        public int Status { get; set; }

        public string? TransactionId { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}
