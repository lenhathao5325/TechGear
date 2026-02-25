using Microsoft.AspNetCore.Mvc;
using TechGearAPI.Data;
using TechGearAPI.Models;

[Route("api/[controller]")]
[ApiController]
public class ProductOptionValuesAPIController : ControllerBase
{
    public class BulkOptionValueDTO
    {
        public int ProductOptionId { get; set; }
        public List<string> Values { get; set; }
    }
    private readonly ApplicationDbContext _context;
    public ProductOptionValuesAPIController(ApplicationDbContext context) => _context = context;

    [HttpGet("option/{optionId}")]
    public IActionResult GetByOption(int optionId)
    {
        var data = _context.productOptionValueAPIs
            .Where(v => v.ProductOptionId == optionId)
            .Select(v => new { v.Id, v.ProductOptionId, v.Value })
            .ToList();
        return Ok(data);
    }

    [HttpPost]
    public IActionResult Create(ProductOptionValueAPI model)
    {
        _context.productOptionValueAPIs.Add(model);
        _context.SaveChanges();
        return Ok(new { message = "Thành công", id = model.Id });
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, ProductOptionValueAPI model)
    {
        var item = _context.productOptionValueAPIs.Find(id);
        if (item == null) return NotFound();
        item.Value = model.Value;
        _context.SaveChanges();
        return Ok(new { message = "Cập nhật thành công" });
    }


    [HttpPost("bulk")]
    public IActionResult CreateBulk(BulkOptionValueDTO dto)
    {
        var option = _context.productOptionAPIs.Find(dto.ProductOptionId);
        if (option == null) return BadRequest("Option không tồn tại");

        var newValues = new List<ProductOptionValueAPI>();
        foreach (var val in dto.Values)
        {
            newValues.Add(new ProductOptionValueAPI
            {
                ProductOptionId = dto.ProductOptionId,
                Value = val
            });
        }

        _context.productOptionValueAPIs.AddRange(newValues);
        _context.SaveChanges();
        return Ok(new { message = $"Đã thêm {newValues.Count} giá trị thành công" });
    }
}