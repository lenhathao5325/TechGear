using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechGearAPI.Data;
using TechGearAPI.DTOs;
using TechGearAPI.Models;

namespace TechGearAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public BrandsController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _context.Brands.Select(b => new BrandDTO { Id = b.Id, Name = b.Name }).ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var b = await _context.Brands.FindAsync(id);
            if (b == null) return NotFound();
            return Ok(new BrandDTO { Id = b.Id, Name = b.Name });
        }

        [HttpPost]
        public async Task<IActionResult> Create(BrandDTO dto)
        {
            var b = new Brand { Name = dto.Name };
            _context.Brands.Add(b);
            await _context.SaveChangesAsync();
            dto.Id = b.Id;
            return CreatedAtAction(nameof(Get), new { id = b.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BrandDTO dto)
        {
            var b = await _context.Brands.FindAsync(id);
            if (b == null) return NotFound();
            b.Name = dto.Name;
            await _context.SaveChangesAsync();
            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var b = await _context.Brands.FindAsync(id);
            if (b == null) return NotFound();
            _context.Brands.Remove(b);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}