using TechGearAPI.Models;

namespace TechGearAPI.Models
{
    public class ProductOptionAPI
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public string Name { get; set; }   // Color, Size...

        // Navigation
        public ProductAPI Product { get; set; }
        public ICollection<ProductOptionValueAPI> ProductOptionValues { get; set; } = new List<ProductOptionValueAPI>();
    }
}
