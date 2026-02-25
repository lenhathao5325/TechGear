namespace TechGearAPI.DTO
{
    public class VariantOptionValueDTO
    {
        public int Id { get; set; } // Id của bảng trung gian
        public int ProductVariantId { get; set; }

        // Hai trường quan trọng để hiển thị:
        public string OptionName { get; set; } // Ví dụ: "Màu sắc", "Kích thước"
        public string Value { get; set; }      // Ví dụ: "Đỏ", "XL"
    }
}
