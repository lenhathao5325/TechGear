using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using TechGearAPI.Models;

namespace TechGearAPI.Models
{
    public class ApplicationUserAPI : IdentityUser
    {
        public string FullName { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public bool IsDeleted { get; set; } = false;

       
        // Navigation
        public ICollection<UserAddressAPI> UserAddresses { get; set; }
        //public ICollection<CartItemAPI> CartItems { get; set; }
        public ICollection<OrderAPI> Orders { get; set; }
      
        public ICollection<CartItemAPI> CartItems { get; set; } = new List<CartItemAPI>();
    }
}
