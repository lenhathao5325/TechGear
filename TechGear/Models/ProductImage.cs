namespace TechGear.Models
{
    public class ProductImage
    {
        public int ProductImageId { get; set; }

        public int? ProductId { get; set; }
        public int? ProductVariantId { get; set; }

        public string ImageUrl { get; set; }
        public string PublicId { get; set; }
        public bool IsPrimary { get; set; }

        // Navigation
        
        public Product Product { get; set; }
        public ProductVariant ProductVariant { get; set; }
    }
}
