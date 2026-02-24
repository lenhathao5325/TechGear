namespace TechGear.Models
{
    /// <summary>
    /// ViewModel for displaying Product Image in MVC views
    /// Data received from API
    /// </summary>
    public class ProductImage
    {
        public int ProductImageId { get; set; }
        public int? ProductId { get; set; }
        public int? ProductVariantId { get; set; }
        public string ImageUrl { get; set; }
        public string? PublicId { get; set; }
        public bool IsPrimary { get; set; }
        
        // For display purposes
        public string? ProductName { get; set; }
        public string? VariantDescription { get; set; }
    }
}
