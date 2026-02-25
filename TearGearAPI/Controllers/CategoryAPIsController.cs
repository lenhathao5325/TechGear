using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechGearAPI.Data;
using TechGearAPI.DTO;
using TechGearAPI.Models;

namespace TearGearAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CategoriesController(ApplicationDbContext context) => _context = context;

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _context.categoryAPIs.Select(c => new { CategoryId = c.CategoryId, CategoryName = c.CategoryName, IsActive = c.IsActive }).ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var c = await _context.categoryAPIs.FindAsync(id);
            if (c == null) return NotFound();
            return Ok(new { CategoryId = c.CategoryId, CategoryName = c.CategoryName, IsActive = c.IsActive });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryDTO dto)
        {
            var c = new CategoryAPI { CategoryName = dto.Name };
            _context.categoryAPIs.Add(c);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = c.CategoryId }, new { CategoryId = c.CategoryId, CategoryName = c.CategoryName });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, CategoryDTO dto)
        {
            var c = await _context.categoryAPIs.FindAsync(id);
            if (c == null) return NotFound();
            c.CategoryName = dto.Name;
            await _context.SaveChangesAsync();
            return Ok(new { CategoryId = c.CategoryId, CategoryName = c.CategoryName });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var c = await _context.categoryAPIs.FindAsync(id);
            if (c == null) return NotFound();
            _context.categoryAPIs.Remove(c);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
