using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechGear.Data;

public class ProductsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var products = _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .OrderByDescending(p => p.Id)
            .Take(12)
            .ToList();
        return View(products);
    }
    public IActionResult ByCategory(int id, int? brandId)
    {
        var productsQuery = _context.Products
            .Include(p => p.Brand)
            .Include(p => p.Category)
            .Where(p => p.CategoryId == id);

        if (brandId.HasValue)
        {
            productsQuery = productsQuery.Where(p => p.BrandId == brandId);
        }

        // 🔥 CHỈ LẤY BRAND CÓ SẢN PHẨM TRONG CATEGORY
        var brands = _context.Products
            .Where(p => p.CategoryId == id)
            .Select(p => p.Brand)
            .Distinct()
            .ToList();

        ViewBag.Brands = brands;
        ViewBag.CategoryId = id;
        ViewBag.SelectedBrand = brandId;

        ViewBag.CategoryName = _context.Categories
            .Where(c => c.CategoryId == id)
            .Select(c => c.CategoryName)
            .FirstOrDefault();

        return View(productsQuery.ToList());
    }


    public IActionResult Details(int id) {
                var product = _context.Products
            .Include(p => p.Category)
            .Include(p => p.Brand)
            .FirstOrDefault(p => p.Id == id);
        if (product == null) {
            return NotFound();
        }
        return View(product);
    }
}
