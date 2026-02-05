using System.Collections.Generic;

namespace TechGearAPI.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }

        public List<ProductImageDTO> ProductImages { get; set; } = new();
    }
} 