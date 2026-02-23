namespace TechGearAPI.Models
{
    public class BrandAPI
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }

        public bool IsActive { get; set; }

        // Navigation
        public ICollection<ProductAPI> Products { get; set; } = new List<ProductAPI>();
    }
}
