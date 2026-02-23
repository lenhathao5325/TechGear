using TearGearAPI.Model;
using static TechGearAPI.Models.Enums.EnumsAPI;

namespace TearGearAPI.DTO
{
    public class ComboDTO
    {
        public string ComboName { get; set; }
        public string? Description { get; set; }

        public decimal OriginalPrice { get; set; }

        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
