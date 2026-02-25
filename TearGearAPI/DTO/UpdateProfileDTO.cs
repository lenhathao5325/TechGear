using System.ComponentModel.DataAnnotations;

namespace TechGearAPI.DTO
{
    public class UpdateProfileDTO
    {
        [Required]
        public string FullName { get; set; }
        
        [Phone]
        public string? PhoneNumber { get; set; }
    }
}
