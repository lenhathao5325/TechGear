namespace TearGearAPI.DTO
{
    public class OrderDTO
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
        public string Status { get; set; }
        // ====== Time ======
        public DateTime OrderDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
