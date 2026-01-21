using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechGear.Models.ViewModels
{
    public class ProductVM
    {
  public int Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc")]
        public string Name { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn danh mục")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn thương hiệu")]
        public int BrandId { get; set; }

        // Upload ảnh
        public IFormFile? ImageFile { get; set; }

        // Dropdowns (KHÔNG BAO GIỜ NULL)
        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Brands { get; set; } = new List<SelectListItem>();

        public bool IsCategoryLocked { get; set; }
        public bool IsBrandLocked { get; set; }
    }
}
