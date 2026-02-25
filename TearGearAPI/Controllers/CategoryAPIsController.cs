using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TearGearAPI.Data;
using TechGearAPI.Models;

namespace TearGearAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryAPIsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryAPIsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CategoryAPIs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryAPI>>> GetCategories()
        {
            return await _context.categoryAPIs.ToListAsync();
        }

        // GET: api/CategoryAPIs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryAPI>> GetCategory(int id)
        {
            var category = await _context.categoryAPIs.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }
    }
}
