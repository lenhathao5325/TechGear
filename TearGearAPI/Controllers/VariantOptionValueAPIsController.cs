using Microsoft.AspNetCore.Mvc;
using TechGearAPI.Data;
using TechGearAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class VariantOptionValueAPIController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public VariantOptionValueAPIController(ApplicationDbContext context) => _context = context;

    // Gán thuộc tính cho biến thể (VD: Biến thể 1 có màu Đỏ)
    [HttpPost]
    public IActionResult AssignOptionToVariant(VariantOptionValueAPI model)
    {
        _context.variantOptionValueAPIs.Add(model);
        _context.SaveChanges();
        return Ok(new { message = "Liên kết thành công" });
    }

    // Lấy thông tin thuộc tính của 1 biến thể
    [HttpGet("variant/{variantId}")]
    public IActionResult GetOptionsByVariant(int variantId)
    {
        var data = _context.variantOptionValueAPIs
            .Where(v => v.ProductVariantId == variantId)
            .Select(v => new {
                v.ProductVariantId,
                v.ProductOptionValueId,
                OptionName = v.ProductOptionValue.ProductOption.Name,
                Value = v.ProductOptionValue.Value
            })
            .ToList();
        return Ok(data);
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var item = _context.variantOptionValueAPIs.Find(id);
        if (item == null) return NotFound();

        _context.variantOptionValueAPIs.Remove(item);
        _context.SaveChanges();
        return Ok(new { message = "Đã hủy liên kết thuộc tính khỏi biến thể" });
    }
}