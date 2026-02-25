namespace TechGearAPI.DTO
{
    public class ProductOptionDTO
    {
        public int Id { get; set; }
        // ====== FK ======
        public int ProductId { get; set; }
        // ====== Info ======
        public string Name { get; set; }
    }
}
