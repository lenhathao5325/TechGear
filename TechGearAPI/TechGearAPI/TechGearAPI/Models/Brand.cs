using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechGearAPI.Models
{
    public class Brand
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}