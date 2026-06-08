using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechGearAPI.Data;
using TechGearAPI.DTO;
using TechGearAPI.Models;

namespace TechGearAPI.Controllers
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
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.ProductVariants)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.ImageUrl,
                    p.CategoryId,
                    p.BrandId,
                    CategoryName = p.Category.CategoryName,
                    BrandName = p.Brand.BrandName,
                    MinPrice = p.ProductVariants.Any() ? p.ProductVariants.Min(v => v.Price) : (decimal?)null,
                    MaxPrice = p.ProductVariants.Any() ? p.ProductVariants.Max(v => v.Price) : (decimal?)null,
                    TotalStock = p.ProductVariants.Any() ? p.ProductVariants.Sum(v => v.Stock) : 0
                })
                .ToListAsync();

            return Ok(products);
        }

        // ================= GET LATEST PRODUCTS =================
        [HttpGet("latest/{count}")]
        public async Task<IActionResult> GetLatestProducts(int count = 12)
        {
            var products = await _context.productAPIs
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.ProductVariants)
                .OrderByDescending(p => p.Id)
                .Take(count)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.ImageUrl,
                    p.CategoryId,
                    p.BrandId,
                    CategoryName = p.Category.CategoryName,
                    BrandName = p.Brand.BrandName,
                    MinPrice = p.ProductVariants.Any() ? p.ProductVariants.Min(v => v.Price) : (decimal?)null,
                    MaxPrice = p.ProductVariants.Any() ? p.ProductVariants.Max(v => v.Price) : (decimal?)null,
                    TotalStock = p.ProductVariants.Any() ? p.ProductVariants.Sum(v => v.Stock) : 0
                })
                .ToListAsync();

            return Ok(products);
        }

        // ================= GET PRODUCTS BY CATEGORY =================
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory(int categoryId)
        {
            var products = await _context.productAPIs
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.ProductVariants)
                .Where(p => p.CategoryId == categoryId)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.ImageUrl,
                    p.CategoryId,
                    p.BrandId,
                    CategoryName = p.Category.CategoryName,
                    BrandName = p.Brand.BrandName,
                    MinPrice = p.ProductVariants.Any() ? p.ProductVariants.Min(v => v.Price) : (decimal?)null,
                    MaxPrice = p.ProductVariants.Any() ? p.ProductVariants.Max(v => v.Price) : (decimal?)null,
                    TotalStock = p.ProductVariants.Any() ? p.ProductVariants.Sum(v => v.Stock) : 0
                })
                .ToListAsync();

            return Ok(products);
        }

        // ================= GET PRODUCTS BY BRAND =================
        [HttpGet("brand/{brandId}")]
        public async Task<IActionResult> GetProductsByBrand(int brandId)
        {
            var products = await _context.productAPIs
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.ProductVariants)
                .Where(p => p.BrandId == brandId)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.ImageUrl,
                    p.CategoryId,
                    p.BrandId,
                    CategoryName = p.Category.CategoryName,
                    BrandName = p.Brand.BrandName,
                    MinPrice = p.ProductVariants.Any() ? p.ProductVariants.Min(v => v.Price) : (decimal?)null,
                    MaxPrice = p.ProductVariants.Any() ? p.ProductVariants.Max(v => v.Price) : (decimal?)null,
                    TotalStock = p.ProductVariants.Any() ? p.ProductVariants.Sum(v => v.Stock) : 0
                })
                .ToListAsync();

            return Ok(products);
        }

        // ================= GET PRODUCT DETAIL =================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.productAPIs
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Images)
                .Include(p => p.ProductVariants)
                .Where(p => p.Id == id)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.ImageUrl,
                    p.CategoryId,
                    p.BrandId,

                    CategoryName = p.Category.CategoryName,
                    BrandName = p.Brand.BrandName,

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

        // ================= SEARCH PRODUCTS =================
        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string q)
        {
            if (string.IsNullOrWhiteSpace(q))
                return Ok(new List<object>());

            var keyword = q.Trim().ToLower();

            var products = await _context.productAPIs
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.ProductVariants)
                .Where(p => p.Name.ToLower().Contains(keyword)
                         || (p.Brand != null && p.Brand.BrandName.ToLower().Contains(keyword))
                         || (p.Category != null && p.Category.CategoryName.ToLower().Contains(keyword)))
                .Take(10)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.ImageUrl,
                    CategoryName = p.Category != null ? p.Category.CategoryName : null,
                    BrandName = p.Brand != null ? p.Brand.BrandName : null,
                    MinPrice = p.ProductVariants.Any() ? p.ProductVariants.Min(v => v.Price) : (decimal?)null,
                    MaxPrice = p.ProductVariants.Any() ? p.ProductVariants.Max(v => v.Price) : (decimal?)null,
                    TotalStock = p.ProductVariants.Any() ? p.ProductVariants.Sum(v => v.Stock) : 0
                })
                .ToListAsync();

            return Ok(products);
        }

        // ================= COMPARE PRODUCTS =================
        [HttpGet("compare")]
        public async Task<IActionResult> Compare([FromQuery] string ids)
        {
            if (string.IsNullOrWhiteSpace(ids))
                return BadRequest("Vui lòng cung cấp danh sách id sản phẩm.");

            var idList = ids.Split(',')
                .Select(s => int.TryParse(s.Trim(), out var n) ? n : 0)
                .Where(n => n > 0)
                .Distinct()
                .Take(4)
                .ToList();

            if (idList.Count < 2)
                return BadRequest("Cần ít nhất 2 sản phẩm để so sánh.");

            var products = await _context.productAPIs
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.ProductVariants)
                .Include(p => p.ProductOptions)
                    .ThenInclude(o => o.ProductOptionValues)
                .Where(p => idList.Contains(p.Id))
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Description,
                    p.ImageUrl,
                    CategoryName = p.Category != null ? p.Category.CategoryName : null,
                    BrandName = p.Brand != null ? p.Brand.BrandName : null,
                    MinPrice = p.ProductVariants.Any() ? p.ProductVariants.Min(v => v.Price) : (decimal?)null,
                    MaxPrice = p.ProductVariants.Any() ? p.ProductVariants.Max(v => v.Price) : (decimal?)null,
                    TotalStock = p.ProductVariants.Any() ? p.ProductVariants.Sum(v => v.Stock) : 0,
                    Options = p.ProductOptions.Select(o => new
                    {
                        OptionName = o.Name,
                        Values = o.ProductOptionValues.Select(v => v.Value)
                    })
                })
                .ToListAsync();

            return Ok(products);
        }
    }
}
