using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TechGear.Models.ViewModels
{
    public class BrandVM
    {
        public int BrandId { get; set; }

        [Required]
        public string BrandName { get; set; }

        public string? Description { get; set; }

        public IFormFile? LogoFile { get; set; }

        public bool IsActive { get; set; }


    }
}
