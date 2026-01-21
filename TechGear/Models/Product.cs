using TechGear.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechGear.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        public int? CategoryId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn thương hiệu")]
        public int? BrandId { get; set; } 
        public string? ImageUrl { get; set; }

        // Navigation
        public Category Category { get; set; }

        public Brand Brand { get; set; }
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
        public ICollection<ProductOption> ProductOptions { get; set; } = new List<ProductOption>();
        public ICollection<ProductVariant> ProductVariants { get; set; } = new List<ProductVariant>();
    }
}
