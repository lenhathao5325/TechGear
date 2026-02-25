using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TearGearAPI.Data;
using TechGearAPI.Models;

namespace TearGearAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandAPIsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public BrandAPIsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/BrandAPIs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BrandAPI>>> GetBrands()
        {
            return await _context.brandAPIs.ToListAsync();
        }

        // GET: api/BrandAPIs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BrandAPI>> GetBrand(int id)
        {
            var brand = await _context.brandAPIs.FindAsync(id);

            if (brand == null)
            {
                return NotFound();
            }

            return brand;
        }
    }
}
