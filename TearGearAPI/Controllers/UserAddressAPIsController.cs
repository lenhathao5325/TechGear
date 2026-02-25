using Microsoft.AspNetCore.Mvc;
using TechGearAPI.Data;
using TechGearAPI.DTO;
using TechGearAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class UserAddressesAPIController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public UserAddressesAPIController(ApplicationDbContext context) => _context = context;

    [HttpGet("user/{userId}")]
    public IActionResult GetByUser(string userId)
    {
        var data = _context.userAddressAPIs
            .Where(a => a.UserId == userId)
            .OrderByDescending(a => a.IsDefault)
            .ToList();
        return Ok(data);
    }

    [HttpPost]
    public IActionResult Create(UserAddressAPI model)
    {
        // Nếu đặt là mặc định, bỏ mặc định các địa chỉ cũ
        if (model.IsDefault)
        {
            var oldDefaults = _context.userAddressAPIs.Where(a => a.UserId == model.UserId && a.IsDefault);
            foreach (var addr in oldDefaults) addr.IsDefault = false;
        }

        _context.userAddressAPIs.Add(model);
        _context.SaveChanges();
        return Ok(new { message = "Thêm địa chỉ thành công", id = model.UserAddressId });
    }

    [HttpPatch("{id}/set-default")]
    public IActionResult SetDefault(int id)
    {
        var address = _context.userAddressAPIs.Find(id);
        if (address == null) return NotFound();

        var allAddresses = _context.userAddressAPIs.Where(a => a.UserId == address.UserId);
        foreach (var addr in allAddresses)
        {
            addr.IsDefault = (addr.UserAddressId == id);
        }

        _context.SaveChanges();
        return Ok(new { message = "Đã đặt làm mặc định" });
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var address = _context.userAddressAPIs.Find(id);
        if (address == null) return NotFound();

        // Kiểm tra nếu địa chỉ đã có trong đơn hàng thì không cho xóa cứng (hoặc báo lỗi)
        bool hasOrder = _context.orderAPIs.Any(o => o.UserAddressId == id);
        if (hasOrder) return BadRequest(new { error = "Địa chỉ này đã được sử dụng trong đơn hàng, không thể xóa." });

        _context.userAddressAPIs.Remove(address);
        _context.SaveChanges();
        return Ok(new { message = "Xóa thành công" });
    }
    // GET: Lấy chi tiết 1 địa chỉ để hiển thị lên Form sửa
    [HttpGet("detail/{id}")]
    public IActionResult GetDetail(int id)
    {
        var address = _context.userAddressAPIs.Find(id);
        if (address == null) return NotFound();
        return Ok(address);
    }

    // PUT: Cập nhật thông tin địa chỉ
    [HttpPut("{id}")]
    public IActionResult Update(int id, UserAddressDTO dto)
    {
        var address = _context.userAddressAPIs.Find(id);
        if (address == null) return NotFound();

        // Cập nhật thông tin
        address.FullName = dto.FullName;
        address.PhoneNumber = dto.PhoneNumber;
        address.AddressLine = dto.AddressLine;
        address.Province = dto.Province;
        address.District = dto.District;
        address.Ward = dto.Ward;
        // Lưu ý: Không update UserId và IsDefault ở đây (IsDefault dùng API riêng)

        _context.SaveChanges();
        return Ok(new { message = "Cập nhật địa chỉ thành công" });
    }
}