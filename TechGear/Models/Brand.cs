namespace TechGear.Models
{
    public class Brand
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }

        public bool IsActive { get; set; }

        // Navigation
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
