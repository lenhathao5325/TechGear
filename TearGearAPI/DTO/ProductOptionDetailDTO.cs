namespace TearGearAPI.DTO
{
    public class ProductOptionDetailDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } // VD: Màu sắc
        public int ProductId { get; set; }

        // Bổ sung danh sách các giá trị con để hiển thị 1 lần là đủ
        public List<ProductOptionValueDTO> Values { get; set; } = new List<ProductOptionValueDTO>();
    }
}
