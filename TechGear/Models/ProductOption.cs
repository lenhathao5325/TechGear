using TechGear.Models;

namespace TechGear.Models
{
    /// <summary>
    /// ViewModel for displaying Product Option in MVC views
    /// Data received from API
    /// </summary>
    public class ProductOption
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; } // e.g., "Color", "Size", "Storage"
        
        // For display purposes
        public string? ProductName { get; set; }
        public List<ProductOptionValue>? Values { get; set; }
    }
}
