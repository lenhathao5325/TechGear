using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechGearAPI.Data;
using TechGearAPI.DTO;
using TechGearAPI.Models;

namespace TearGearAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductImagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ProductImagesController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _context.productImageAPIs.Select(i => new ProductImageDTO {
            Id = i.ProductImageId, Url = i.ImageUrl, ProductId = i.ProductId ?? 0
        }).ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var i = await _context.productImageAPIs.FindAsync(id);
            if (i == null) return NotFound();
            return Ok(new ProductImageDTO { Id = i.ProductImageId, Url = i.ImageUrl, ProductId = i.ProductId ?? 0 });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductImageDTO dto)
        {
            var i = new ProductImageAPI { ImageUrl = dto.Url, ProductId = dto.ProductId };
            _context.productImageAPIs.Add(i);
            await _context.SaveChangesAsync();
            dto.Id = i.ProductImageId;
            return CreatedAtAction(nameof(Get), new { id = i.ProductImageId }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductImageDTO dto)
        {
            var i = await _context.productImageAPIs.FindAsync(id);
            if (i == null) return NotFound();
            i.ImageUrl = dto.Url; i.ProductId = dto.ProductId;
            await _context.SaveChangesAsync();
            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var i = await _context.productImageAPIs.FindAsync(id);
            if (i == null) return NotFound();
            _context.productImageAPIs.Remove(i);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}