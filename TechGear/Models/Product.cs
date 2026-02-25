using System.ComponentModel.DataAnnotations;

namespace TechGear.Models
{
    /// <summary>
    /// ViewModel for displaying Product information in MVC views
    /// Data received from API
    /// </summary>
    public class Product
    {
        public int Id { get; set; }
        public int ProductId 
        { 
            get => Id; 
            set => Id = value; 
        }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        public string Name { get; set; }
        
        public string ProductName 
        { 
            get => Name; 
            set => Name = value; 
        }
        
        public string? Description { get; set; }
        
        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        public int? CategoryId { get; set; }
        
        [Required(ErrorMessage = "Vui lòng chọn thương hiệu")]
        public int? BrandId { get; set; }
        
        public string? ImageUrl { get; set; }
        
        [Required(ErrorMessage = "Vui lòng nhập giá sản phẩm")]
        public decimal BasePrice { get; set; }
        
        public bool IsActive { get; set; } = true;
        
        // Flattened properties from related entities for display
        public string? CategoryName { get; set; }
        public string? BrandName { get; set; }
        
        // For product listing/details views
        public List<string>? ImageUrls { get; set; }
        public List<ProductVariant>? Variants { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public int? TotalStock { get; set; }
    }
}
