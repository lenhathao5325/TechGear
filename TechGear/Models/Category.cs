using System.ComponentModel.DataAnnotations;

namespace TechGear.Models
{
    /// <summary>
    /// ViewModel for displaying Category information in MVC views
    /// Data received from API
    /// </summary>
    public class Category
    {
        public int CategoryId { get; set; }
        
        [Required(ErrorMessage = "Tên danh mục không được để trống")]
        public string CategoryName { get; set; }
        
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        
        // For display purposes only
        public int? ProductCount { get; set; }
    }
}
