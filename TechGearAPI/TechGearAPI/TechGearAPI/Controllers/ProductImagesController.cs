using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechGearAPI.Data;
using TechGearAPI.DTOs;
using TechGearAPI.Models;

namespace TechGearAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductImagesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ProductImagesController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _context.ProductImages.Select(i => new ProductImageDTO {
            Id = i.Id, Url = i.Url, ProductId = i.ProductId
        }).ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var i = await _context.ProductImages.FindAsync(id);
            if (i == null) return NotFound();
            return Ok(new ProductImageDTO { Id = i.Id, Url = i.Url, ProductId = i.ProductId });
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductImageDTO dto)
        {
            var i = new ProductImage { Url = dto.Url, ProductId = dto.ProductId };
            _context.ProductImages.Add(i);
            await _context.SaveChangesAsync();
            dto.Id = i.Id;
            return CreatedAtAction(nameof(Get), new { id = i.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductImageDTO dto)
        {
            var i = await _context.ProductImages.FindAsync(id);
            if (i == null) return NotFound();
            i.Url = dto.Url; i.ProductId = dto.ProductId;
            await _context.SaveChangesAsync();
            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var i = await _context.ProductImages.FindAsync(id);
            if (i == null) return NotFound();
            _context.ProductImages.Remove(i);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}