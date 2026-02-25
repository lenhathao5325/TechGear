using TechGearAPI.Models.Enums;
using static TechGearAPI.Models.Enums.EnumsAPI;


namespace TechGearAPI.Service.Helpers
{
    public static class ComboPriceCalculator
    {
        public static decimal Calculate(
            decimal originalPrice,
            DiscountType discountType,
            decimal discountValue)
        {
            return discountType switch
            {
                DiscountType.Percentage =>
                    originalPrice - (originalPrice * discountValue / 100),

                DiscountType.FixedAmount =>
                    Math.Max(0, originalPrice - discountValue),

                _ => originalPrice
            };
        }
    }
}
