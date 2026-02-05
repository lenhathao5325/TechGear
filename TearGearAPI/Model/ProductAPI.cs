using TechGearAPI.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechGearAPI.Models
{
    public class ProductAPI
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
        public CategoryAPI?  Category { get; set; }

        public BrandAPI? Brand { get; set; }
        public ICollection<ProductImageAPI> Images { get; set; } = new List<ProductImageAPI>();
        public ICollection<ProductOptionAPI> ProductOptions { get; set; } = new List<ProductOptionAPI>();
        public ICollection<ProductVariantAPI> ProductVariants { get; set; } = new List<ProductVariantAPI>();
    }
}
