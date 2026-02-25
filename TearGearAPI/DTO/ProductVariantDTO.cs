namespace TearGearAPI.DTO
{
    public class ProductVariantDTO
    {
        public int Id { get; set; }
        public string SKU { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int ProductId { get; set; }
    }
}