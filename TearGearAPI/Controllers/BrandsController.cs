using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechGearAPI.Data;
using TechGearAPI.DTO;
using TechGearAPI.DTOs;
using TechGearAPI.Models;

namespace TearGearAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BrandsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public BrandsController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _context.brandAPIs.Select(b => new { BrandId = b.BrandId, BrandName = b.BrandName, IsActive = b.IsActive }).ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var b = await _context.brandAPIs.FindAsync(id);
            if (b == null) return NotFound();
            return Ok(new { BrandId = b.BrandId, BrandName = b.BrandName, IsActive = b.IsActive });
        }

        [HttpPost]
        public async Task<IActionResult> Create(BrandDTO dto)
        {
            var b = new BrandAPI { BrandName = dto.Name };
            _context.brandAPIs.Add(b);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = b.BrandId }, new { BrandId = b.BrandId, BrandName = b.BrandName });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BrandDTO dto)
        {
            var b = await _context.brandAPIs.FindAsync(id);
            if (b == null) return NotFound();
            b.BrandName = dto.Name;
            await _context.SaveChangesAsync();
            return Ok(new { BrandId = b.BrandId, BrandName = b.BrandName });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var b = await _context.brandAPIs.FindAsync(id);
            if (b == null) return NotFound();
            _context.brandAPIs.Remove(b);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}