namespace TechGearAPI.Models
{
    public class ProductImageAPI
    {
        public int ProductImageId { get; set; }

        public int? ProductId { get; set; }
        public int? ProductVariantId { get; set; }

        public string ImageUrl { get; set; }
        public string PublicId { get; set; }
        public bool IsPrimary { get; set; }

        // Navigation
        
        public ProductAPI Product { get; set; }
        public ProductVariantAPI ProductVariant { get; set; }
    }
}
