namespace TearGearAPI.DTO
{
    public class ProductOptionValueDTO
    {
        public int Id { get; set; }
        // ====== FK ======
        public int ProductOptionId { get; set; }
        // ====== Info ======
        public string Value { get; set; }
    }
}
