namespace TearGearAPI.DTO
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public string? ImageUrl { get; set; }

    }
}
