namespace TearGearAPI.DTO
{
    public class UserAddressDTO
    {
        public int UserAddressId { get; set; }
        // ====== FK ======
        public string UserId { get; set; }
        // ====== Contact Info ======
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        // ====== Location ======
        public string AddressLine { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        // ====== Status ======
        public bool IsDefault { get; set; }
    }
}
