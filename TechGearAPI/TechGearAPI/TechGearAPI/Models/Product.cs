using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechGearAPI.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int? CategoryId { get; set; }
        public Category Category { get; set; }

        public int? BrandId { get; set; }
        public Brand Brand { get; set; }

        public ICollection<ProductVariant> ProductVariants { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }
    }
}