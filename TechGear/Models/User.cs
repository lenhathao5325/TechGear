namespace TechGear.Models
{
    /// <summary>
    /// ViewModel for displaying User information in MVC views
    /// Data received from API
    /// </summary>
    public class User
    {
        public string UserId { get; set; } // Changed from int to string to match Identity
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime? CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
        
        // For display purposes
        public List<UserAddress>? Addresses { get; set; }
        public int? TotalOrders { get; set; }
    }
}
