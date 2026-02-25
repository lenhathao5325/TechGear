using static TechGearAPI.Models.Enums.EnumsAPI;

namespace TechGearAPI.Models
{
    public class ComboAPI
    {
        public int ComboId { get; set; }

        public int ComboItemId { get; set; }

        public string ComboName { get; set; } = null!;
        public string? Description { get; set; }

        public decimal OriginalPrice { get; set; }

        // Giá sau khi áp d?ng gi?m
        public decimal FinalPrice { get; set; }

        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }


        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public ICollection<ComboItemAPI> ComboItems { get; set; } = new List<ComboItemAPI>();
    }
}

