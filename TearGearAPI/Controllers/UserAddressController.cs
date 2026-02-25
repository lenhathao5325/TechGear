using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TechGearAPI.Data;
using TechGearAPI.Models;

namespace TechGearAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserAddressController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserAddressController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/useraddress
        [HttpGet]
        public async Task<IActionResult> GetUserAddresses()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var addresses = await _context.userAddressAPIs
                .Where(a => a.UserId == userId)
                .Select(a => new
                {
                    userAddressId = a.UserAddressId,
                    userId = a.UserId,
                    recipientName = a.FullName,
                    phoneNumber = a.PhoneNumber,
                    addressLine = a.AddressLine,
                    ward = a.Ward,
                    district = a.District,
                    city = a.Province,
                    isDefault = a.IsDefault,
                    fullAddress = $"{a.AddressLine}, {a.Ward}, {a.District}, {a.Province}"
                })
                .ToListAsync();

            return Ok(addresses);
        }

        // GET: api/useraddress/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserAddress(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var address = await _context.userAddressAPIs
                .FirstOrDefaultAsync(a => a.UserAddressId == id && a.UserId == userId);

            if (address == null)
                return NotFound(new { message = "Không tìm thấy địa chỉ!" });

            return Ok(new
            {
                userAddressId = address.UserAddressId,
                userId = address.UserId,
                recipientName = address.FullName,
                phoneNumber = address.PhoneNumber,
                addressLine = address.AddressLine,
                ward = address.Ward,
                district = address.District,
                city = address.Province,
                isDefault = address.IsDefault
            });
        }

        // POST: api/useraddress
        [HttpPost]
        public async Task<IActionResult> CreateUserAddress([FromBody] UserAddressRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // If this is set as default, unset other defaults
            if (request.IsDefault)
            {
                var existingAddresses = await _context.userAddressAPIs
                    .Where(a => a.UserId == userId && a.IsDefault)
                    .ToListAsync();

                foreach (var addr in existingAddresses)
                {
                    addr.IsDefault = false;
                }
            }

            var address = new UserAddressAPI
            {
                UserId = userId,
                FullName = request.RecipientName,
                PhoneNumber = request.PhoneNumber,
                AddressLine = request.AddressLine,
                Ward = request.Ward,
                District = request.District,
                Province = request.City,
                IsDefault = request.IsDefault
            };

            _context.userAddressAPIs.Add(address);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Thêm địa chỉ thành công!", userAddressId = address.UserAddressId });
        }

        // PUT: api/useraddress/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAddress(int id, [FromBody] UserAddressRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var address = await _context.userAddressAPIs
                .FirstOrDefaultAsync(a => a.UserAddressId == id && a.UserId == userId);

            if (address == null)
                return NotFound(new { message = "Không tìm thấy địa chỉ!" });

            // If this is set as default, unset other defaults
            if (request.IsDefault && !address.IsDefault)
            {
                var existingAddresses = await _context.userAddressAPIs
                    .Where(a => a.UserId == userId && a.IsDefault && a.UserAddressId != id)
                    .ToListAsync();

                foreach (var addr in existingAddresses)
                {
                    addr.IsDefault = false;
                }
            }

            address.FullName = request.RecipientName;
            address.PhoneNumber = request.PhoneNumber;
            address.AddressLine = request.AddressLine;
            address.Ward = request.Ward;
            address.District = request.District;
            address.Province = request.City;
            address.IsDefault = request.IsDefault;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Cập nhật địa chỉ thành công!" });
        }

        // DELETE: api/useraddress/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAddress(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var address = await _context.userAddressAPIs
                .FirstOrDefaultAsync(a => a.UserAddressId == id && a.UserId == userId);

            if (address == null)
                return NotFound(new { message = "Không tìm thấy địa chỉ!" });

            _context.userAddressAPIs.Remove(address);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Xóa địa chỉ thành công!" });
        }

        // PUT: api/useraddress/{id}/set-default
        [HttpPut("{id}/set-default")]
        public async Task<IActionResult> SetDefaultAddress(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var address = await _context.userAddressAPIs
                .FirstOrDefaultAsync(a => a.UserAddressId == id && a.UserId == userId);

            if (address == null)
                return NotFound(new { message = "Không tìm thấy địa chỉ!" });

            // Unset all other defaults
            var existingAddresses = await _context.userAddressAPIs
                .Where(a => a.UserId == userId && a.IsDefault)
                .ToListAsync();

            foreach (var addr in existingAddresses)
            {
                addr.IsDefault = false;
            }

            address.IsDefault = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đặt địa chỉ mặc định thành công!" });
        }
    }

    public class UserAddressRequest
    {
        public string RecipientName { get; set; }
        public string PhoneNumber { get; set; }
        public string AddressLine { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public bool IsDefault { get; set; } = false;
    }
}
