using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechGearAPI.Data;
using TearGearAPI.DTO;
using TechGearAPI.Models;

namespace TearGearAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductVariantsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductVariantsController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _context.productVariantAPIs.Select(v => new   ProductVariantDTO {
            Id = v.Id, SKU = v.SKU, Price = v.Price, Stock = v.Stock, ProductId = v.ProductId
        }).ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var v = await _context.productVariantAPIs.FindAsync(id);
            if (v == null) return NotFound();
            return Ok(new ProductVariantDTO { Id = v.Id, SKU = v.SKU, Price = v.Price, Stock = v.Stock, ProductId = v.ProductId });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductVariantDTO dto)
        {
            var v = new ProductVariantAPI { SKU = dto.SKU, Price = dto.Price, Stock = dto.Stock, ProductId = dto.ProductId };
            _context.productVariantAPIs.Add(v);
            await _context.SaveChangesAsync();
            dto.Id = v.Id;
            return CreatedAtAction(nameof(Get), new { id = v.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductVariantDTO dto)
        {
            var v = await _context.productVariantAPIs.FindAsync(id);
            if (v == null) return NotFound();
            v.SKU = dto.SKU; v.Price = dto.Price; v.Stock = dto.Stock; v.ProductId = dto.ProductId;
            await _context.SaveChangesAsync();
            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var v = await _context.productVariantAPIs.FindAsync(id);
            if (v == null) return NotFound();
            _context.productVariantAPIs.Remove(v);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}