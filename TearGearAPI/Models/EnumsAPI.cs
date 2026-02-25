namespace TechGearAPI.Models.Enums
{
    public class EnumsAPI
    {
        public enum OrderStatus
        {
            Pending = 1,
            Confirmed = 2,
            Shipping = 3,
            Completed = 4,
            Cancelled = 5
        }

        public enum PaymentStatus
        {
            Pending = 1,
            Paid = 2,
            Failed = 3
        }

        public enum PaymentMethod
        {
            COD = 1,
            Banking = 2,
            Momo = 3,
            VNPay = 4
        }

        public enum DiscountType
        {
            None = 0,
            Percentage = 1,
            FixedAmount = 2
        }
    }



}
