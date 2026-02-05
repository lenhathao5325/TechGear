using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechGearAPI.Data;
using TechGearAPI.DTOs;
using TechGearAPI.Models;

namespace TechGearAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductVariantsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductVariantsController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _context.ProductVariants.Select(v => new ProductVariantDTO {
            Id = v.Id, SKU = v.SKU, Price = v.Price, Stock = v.Stock, ProductId = v.ProductId
        }).ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var v = await _context.ProductVariants.FindAsync(id);
            if (v == null) return NotFound();
            return Ok(new ProductVariantDTO { Id = v.Id, SKU = v.SKU, Price = v.Price, Stock = v.Stock, ProductId = v.ProductId });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductVariantDTO dto)
        {
            var v = new ProductVariant { SKU = dto.SKU, Price = dto.Price, Stock = dto.Stock, ProductId = dto.ProductId };
            _context.ProductVariants.Add(v);
            await _context.SaveChangesAsync();
            dto.Id = v.Id;
            return CreatedAtAction(nameof(Get), new { id = v.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductVariantDTO dto)
        {
            var v = await _context.ProductVariants.FindAsync(id);
            if (v == null) return NotFound();
            v.SKU = dto.SKU; v.Price = dto.Price; v.Stock = dto.Stock; v.ProductId = dto.ProductId;
            await _context.SaveChangesAsync();
            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var v = await _context.ProductVariants.FindAsync(id);
            if (v == null) return NotFound();
            _context.ProductVariants.Remove(v);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}