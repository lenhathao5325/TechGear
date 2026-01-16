using TechGear.Models;

namespace TechGear.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public string? Description { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; } = 0;

        public int ProductImages { get; set; }

        // Navigation
        public Category Category { get; set; }

        public Brand Brand { get; set; }
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<ProductOption> ProductOptions { get; set; } = new List<ProductOption>();
        public ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
    }
}
