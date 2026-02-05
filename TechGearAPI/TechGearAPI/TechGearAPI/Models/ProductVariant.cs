using System.ComponentModel.DataAnnotations;

namespace TechGearAPI.Models
{
    public class ProductVariant
    {
        public int Id { get; set; }

        [Required]
        public string SKU { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}