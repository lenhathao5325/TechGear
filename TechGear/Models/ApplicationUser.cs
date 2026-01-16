using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using TechGear.Models;

namespace TechGear.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;

       
        // Navigation
        public ICollection<UserAddress> UserAddresses { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}
