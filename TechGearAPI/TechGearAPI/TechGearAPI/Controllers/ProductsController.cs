using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechGearAPI.Data;
using TechGearAPI.DTOs;
using TechGearAPI.Models;

namespace TechGearAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductVariants)
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    CategoryId = p.CategoryId,
                    BrandId = p.BrandId,
                    ProductImages = p.ProductImages.Select(i => new ProductImageDTO { Id = i.Id, Url = i.Url, ProductId = i.ProductId }).ToList()
                }).ToListAsync();

            return Ok(list);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var p = await _context.Products
                .Include(x => x.ProductImages)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (p == null) return NotFound();

            var dto = new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                CategoryId = p.CategoryId,
                BrandId = p.BrandId,
                ProductImages = p.ProductImages.Select(i => new ProductImageDTO { Id = i.Id, Url = i.Url, ProductId = i.ProductId }).ToList()
            };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductDTO dto)
        {
            var p = new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                CategoryId = dto.CategoryId,
                BrandId = dto.BrandId
            };
            _context.Products.Add(p);
            await _context.SaveChangesAsync();
            dto.Id = p.Id;
            return CreatedAtAction(nameof(Get), new { id = p.Id }, dto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductDTO dto)
        {
            var p = await _context.Products.FindAsync(id);
            if (p == null) return NotFound();

            p.Name = dto.Name;
            p.Description = dto.Description;
            p.Price = dto.Price;
            p.CategoryId = dto.CategoryId;
            p.BrandId = dto.BrandId;

            await _context.SaveChangesAsync();
            return Ok(dto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _context.Products.FindAsync(id);
            if (p == null) return NotFound();

            _context.Products.Remove(p);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}