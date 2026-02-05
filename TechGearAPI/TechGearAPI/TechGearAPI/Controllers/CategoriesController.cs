using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechGearAPI.Data;
using TechGearAPI.DTOs;
using TechGearAPI.Models;

namespace TechGearAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public CategoriesController(AppDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _context.Categories.Select(c => new CategoryDTO { Id = c.Id, Name = c.Name }).ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var c = await _context.Categories.FindAsync(id);
            if (c == null) return NotFound();
            return Ok(new CategoryDTO { Id = c.Id, Name = c.Name });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDTO dto)
        {
            var c = new Category { Name = dto.Name };
            _context.Categories.Add(c);
            await _context.SaveChangesAsync();
            dto.Id = c.Id;
            return CreatedAtAction(nameof(Get), new { id = c.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryDTO dto)
        {
            var c = await _context.Categories.FindAsync(id);
            if (c == null) return NotFound();
            c.Name = dto.Name;
            await _context.SaveChangesAsync();
            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var c = await _context.Categories.FindAsync(id);
            if (c == null) return NotFound();
            _context.Categories.Remove(c);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}