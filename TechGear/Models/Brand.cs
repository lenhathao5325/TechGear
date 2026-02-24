using System.ComponentModel.DataAnnotations;

namespace TechGear.Models
{
    /// <summary>
    /// ViewModel for displaying Brand information in MVC views
    /// Data received from API
    /// </summary>
    public class Brand
    {
        public int BrandId { get; set; }
        
        [Required(ErrorMessage = "Tên thương hiệu không được để trống")]
        public string BrandName { get; set; }
        
        public string? Description { get; set; }
        public string? LogoUrl { get; set; }
        public bool IsActive { get; set; }
        
        // For display purposes only - populated from API response
        public int? ProductCount { get; set; }
    }
}
