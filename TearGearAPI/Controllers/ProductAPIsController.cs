using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TearGearAPI.Data;
using TechGearAPI.DTO;
using TechGearAPI.Models;

namespace TearGearAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductAPIsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProductAPIsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= GET ALL PRODUCTS =================
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.productAPIs
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.ImageUrl,

                    Category = p.Category.CategoryName,
                    Brand = p.Brand.BrandName
                })
                .ToListAsync();

            return Ok(products);
        }

        // ================= GET PRODUCT DETAIL =================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.productAPIs
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.ImageUrl,
                    p.CategoryId,
                    p.BrandId,

                    Category = p.Category.CategoryName,
                    Brand = p.Brand.BrandName,

                    Images = p.Images.Select(i => new
                    {
                        i.ImageUrl
                    }),

                    Variants = p.ProductVariants.Select(v => new
                    {
                        v.Id,
                        v.Price,
                    })
                })
                .FirstOrDefaultAsync();

            if (product == null)
                return NotFound();

            return Ok(product);
        }

        // ================= CREATE PRODUCT =================
        [HttpPost]
        public async Task<IActionResult> Create(ProductDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryExists = await _context.categoryAPIs
                .AnyAsync(c => c.CategoryId == dto.CategoryId);

            if (!categoryExists)
                return BadRequest("Category không tồn tại");

            var brandExists = await _context.brandAPIs
                .AnyAsync(b => b.BrandId == dto.BrandId);

            if (!brandExists)
                return BadRequest("Brand không tồn tại");

            var product = new ProductAPI
            {
                Name = dto.Name,
                Description = dto.Description,
                CategoryId = dto.CategoryId,
                BrandId = dto.BrandId,
                ImageUrl = dto.ImageUrl
            };

            _context.productAPIs.Add(product);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Tạo sản phẩm thành công",
                product
            });
        }

        // ================= UPDATE PRODUCT =================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, ProductDTO dto)
        {
            var product = await _context.productAPIs.FindAsync(id);

            if (product == null)
                return NotFound();

            product.Name = dto.Name;
            product.Description = dto.Description;
            product.CategoryId = dto.CategoryId;
            product.BrandId = dto.BrandId;
            product.ImageUrl = dto.ImageUrl;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Cập nhật thành công",
                product
            });
        }

        // ================= DELETE PRODUCT =================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await _context.productAPIs.FindAsync(id);

            if (product == null)
                return NotFound();

            _context.productAPIs.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Đã xoá sản phẩm" });
        }
    }
}
