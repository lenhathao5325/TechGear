namespace TechGearAPI.Models
{
    public class CategoryAPI
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string? Description { get; set; }

        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation
        public ICollection<ProductAPI> Products { get; set; } = new List<ProductAPI>();
    }
}
