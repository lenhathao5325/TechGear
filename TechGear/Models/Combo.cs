using static TechGear.Models.Enums.Enums;

namespace TechGear.Models
{
    /// <summary>
    /// ViewModel for displaying Combo information in MVC views
    /// Data received from API
    /// </summary>
    public class Combo
    {
        public int ComboId { get; set; }
        public string ComboName { get; set; }
        public string? Description { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal FinalPrice { get; set; }
        
        // Discount information
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public string? DiscountText { get; set; } // e.g., "Giảm 10%" or "Giảm 100.000đ"
        
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        
        // For display purposes
        public List<ComboItem>? Items { get; set; }
        public decimal SavingsAmount => OriginalPrice - FinalPrice;
    }
}

