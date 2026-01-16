using static TechGear.Models.Enums.Enums;

namespace TechGear.Models
{
    public class Combo
    {
        public int ComboId { get; set; }

        public int ComboItemId { get; set; }

        public string ComboName { get; set; } = null!;
        public string? Description { get; set; }

        public decimal OriginalPrice { get; set; }

        // Giá sau khi áp dụng giảm
        public decimal FinalPrice { get; set; }

        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }


        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public ICollection<ComboItem> ComboItems { get; set; } = new List<ComboItem>();
    }
}

