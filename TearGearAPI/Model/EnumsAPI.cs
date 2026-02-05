namespace TechGearAPI.Models.Enums
{
    public class EnumsAPI
    {
        public enum OrderStatus
        {
            Pending = 1,      // Chờ xử lý
            Confirmed = 2,    // Đã xác nhận
            Shipping = 3,     // Đang giao
            Completed = 4,    // Hoàn tất
            Cancelled = 5     // Đã hủy
        }

        public enum PaymentStatus
        {
            Pending = 1,
            Paid = 2,
            Failed = 3
        }

        public enum DiscountType
        {
            None = 0,
            Percentage = 1,   // giảm %
            FixedAmount = 2   // giảm tiền cố định
        }


    }

}
