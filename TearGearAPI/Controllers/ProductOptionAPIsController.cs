using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechGearAPI.Data;
using TechGearAPI.DTO;
using TechGearAPI.Models;

namespace TechGearAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductOptionsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductOptionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ProductOptions/ByProduct/5
        [HttpGet("ByProduct/{productId}")]
        public IActionResult GetByProductId(int productId)
        {
            var options = _context.productOptionAPIs // Giả định tên DbSet trong context
                .Where(x => x.ProductId == productId)
                .Select(x => new
                {
                    x.Id,
                    x.ProductId,
                    x.Name,
                    Values = x.ProductOptionValues.Select(v => new { v.Id, v.Value }).ToList()
                })
                .ToList();
            return Ok(options);
        }

        // POST: api/ProductOptions
        [HttpPost]
        public IActionResult Create(ProductOptionDTO dto)
        {
            // Kiểm tra Product có tồn tại không (nếu cần thiết)
            // var productExists = ...

            var option = new ProductOptionAPI
            {
                ProductId = dto.ProductId,
                Name = dto.Name
            };

            _context.productOptionAPIs.Add(option);
            _context.SaveChanges();

            return Ok(new { message = "Tạo Option thành công", id = option.Id });
        }

        // DELETE: api/ProductOptions/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var option = _context.productOptionAPIs.Find(id);
            if (option == null) return NotFound(new { error = "Option không tồn tại" });

            _context.productOptionAPIs.Remove(option);
            _context.SaveChanges();
            return Ok(new { message = "Đã xóa Option thành công" });
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] string newName)
        {
            var option = _context.productOptionAPIs.Find(id);
            if (option == null) return NotFound();

            option.Name = newName;
            _context.SaveChanges();
            return Ok(new { message = "Đã sửa tên Option" });
        }
    }
}