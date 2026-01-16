using TechGear.Models;

namespace TechGear.Models
{
    public class ProductOption
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public string Name { get; set; }   // Color, Size...

        // Navigation
        public Product Product { get; set; }
        public ICollection<ProductOptionValue> ProductOptionValues { get; set; } = new List<ProductOptionValue>();
    }
}
