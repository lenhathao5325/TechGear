using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TearGearAPI.Data;
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
        public async Task<IActionResult> GetAll() => Ok(await _context.brandAPIs.Select(b => new BrandDTO { Id = b.BrandId, Name = b.BrandName }).ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var b = await _context.brandAPIs.FindAsync(id);
            if (b == null) return NotFound();
            return Ok(new BrandDTO { Id = b.BrandId, Name = b.BrandName });
        }

        [HttpPost]
        public async Task<IActionResult> Create(BrandDTO dto)
        {
            var b = new BrandAPI { BrandName = dto.Name };
            _context.brandAPIs.Add(b);
            await _context.SaveChangesAsync();
            dto.Id = b.BrandId;
            return CreatedAtAction(nameof(Get), new { id = b.BrandId }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, BrandDTO dto)
        {
            var b = await _context.brandAPIs.FindAsync(id);
            if (b == null) return NotFound();
            b.BrandName = dto.Name;
            await _context.SaveChangesAsync();
            return Ok(dto);
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