using TechGear.Models.Enums;
using static TechGear.Models.Enums.Enums;

namespace TechGear.Services.Helpers
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
