using Microsoft.AspNetCore.Mvc;
using TechGear.Models;
using TechGear.Services;

namespace TechGear.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IApiService _apiService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IApiService apiService, ILogger<ProductsController> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _apiService.GetProductsAsync();
            return View(products);
        }

        public async Task<IActionResult> ByCategory(int id, int? brandId)
        {
            var products = await _apiService.GetProductsByCategoryAsync(id);

            // Filter by brand if specified
            if (brandId.HasValue)
            {
                products = products.Where(p => p.BrandId == brandId).ToList();
            }

            // Get brands for filter (distinct brands from products)
            var brands = products
                .Select(p => new Brand
                {
                    BrandId = p.BrandId ?? 0,
                    BrandName = p.BrandName
                })
                .DistinctBy(b => b.BrandId)
                .Where(b => b.BrandId > 0)
                .ToList();

            // Get category name
            var category = await _apiService.GetCategoryByIdAsync(id);
            string categoryName = category?.CategoryName ?? "Danh mục";

            ViewBag.Brands = brands;
            ViewBag.CategoryId = id;
            ViewBag.SelectedBrand = brandId;
            ViewBag.CategoryName = categoryName;

            return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _apiService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
    }
}
