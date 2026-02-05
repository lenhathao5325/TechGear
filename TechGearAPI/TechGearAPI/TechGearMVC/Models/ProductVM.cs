using System.Collections.Generic;

namespace TechGearMVC.Models
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int? CategoryId { get; set; }
        public int? BrandId { get; set; }

        // initialize lists to avoid nullable warnings and to simplify view rendering
        public List<CategoryVM> Categories { get; set; } = new();
        public List<BrandVM> Brands { get; set; } = new();
        public List<ProductImageVM> ProductImages { get; set; } = new();
    }

    public class CategoryVM { public int Id { get; set; } public string Name { get; set; } = string.Empty; }
    public class BrandVM { public int Id { get; set; } public string Name { get; set; } = string.Empty; }
    public class ProductImageVM { public int Id { get; set; } public string Url { get; set; } = string.Empty; }
} 