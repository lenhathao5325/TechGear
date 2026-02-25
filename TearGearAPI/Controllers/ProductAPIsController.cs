using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TearGearAPI.Data;
using TechGearAPI.Models;

namespace TearGearAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductAPIsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductAPIsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ProductAPIs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetProducts()
        {
            var products = await _context.productAPIs
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.CategoryId,
                    p.BrandId,
                    p.ImageUrl,
                    CategoryName = p.Category.CategoryName,
                    BrandName = p.Brand.BrandName
                })
                .ToListAsync();

            return Ok(products);
        }

        // GET: api/ProductAPIs/latest/12
        [HttpGet("latest/{count}")]
        public async Task<ActionResult<IEnumerable<object>>> GetLatestProducts(int count = 12)
        {
            var products = await _context.productAPIs
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .OrderByDescending(p => p.Id)
                .Take(count)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.CategoryId,
                    p.BrandId,
                    p.ImageUrl,
                    CategoryName = p.Category.CategoryName,
                    BrandName = p.Brand.BrandName
                })
                .ToListAsync();

            return Ok(products);
        }

        // GET: api/ProductAPIs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetProduct(int id)
        {
            var product = await _context.productAPIs
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.CategoryId,
                    p.BrandId,
                    p.ImageUrl,
                    CategoryName = p.Category.CategoryName,
                    BrandName = p.Brand.BrandName
                })
                .FirstOrDefaultAsync();

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        // GET: api/ProductAPIs/category/5
        [HttpGet("category/{categoryId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetProductsByCategory(int categoryId)
        {
            var products = await _context.productAPIs
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Where(p => p.CategoryId == categoryId)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.CategoryId,
                    p.BrandId,
                    p.ImageUrl,
                    CategoryName = p.Category.CategoryName,
                    BrandName = p.Brand.BrandName
                })
                .ToListAsync();

            return Ok(products);
        }

        // GET: api/ProductAPIs/brand/5
        [HttpGet("brand/{brandId}")]
        public async Task<ActionResult<IEnumerable<object>>> GetProductsByBrand(int brandId)
        {
            var products = await _context.productAPIs
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Where(p => p.BrandId == brandId)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.CategoryId,
                    p.BrandId,
                    p.ImageUrl,
                    CategoryName = p.Category.CategoryName,
                    BrandName = p.Brand.BrandName
                })
                .ToListAsync();

            return Ok(products);
        }
    }
}
