using TechGear.Models;

namespace TechGear.Services
{
    public interface IApiService
    {
        Task<List<Product>> GetProductsAsync();
        Task<List<Product>> GetLatestProductsAsync(int count = 12);
        Task<Product?> GetProductByIdAsync(int id);
        Task<List<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<List<Product>> GetProductsByBrandAsync(int brandId);
        Task<List<Category>> GetCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        Task<List<Brand>> GetBrandsAsync();
        Task<Brand?> GetBrandByIdAsync(int id);
    }
}
