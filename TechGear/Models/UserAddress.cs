using CloudinaryDotNet.Actions;
using TechGear.Models;

namespace TechGear.Models
{
    /// <summary>
    /// ViewModel for displaying User Address in MVC views
    /// Data received from API
    /// </summary>
    public class UserAddress 
    {
        public int UserAddressId { get; set; }
        public string UserId { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressLine { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public bool IsDefault { get; set; }
        
        // For display purposes
        public string FullAddress => $"{AddressLine}, {Ward}, {District}, {Province}";
    }
}
