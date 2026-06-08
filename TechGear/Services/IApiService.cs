using TechGear.Models;

namespace TechGear.Services
{
    public interface IApiService
    {
        // Generic methods for API calls
        Task<T?> GetAsync<T>(string endpoint);
        Task<T?> PostAsync<T>(string endpoint, object data);
        Task<T?> PostMultipartAsync<T>(string endpoint, MultipartFormDataContent formData);
        Task<bool> PutAsync<T>(string endpoint, object data);
        Task<bool> DeleteAsync(string endpoint);
        
        // Product methods
        Task<List<Product>> GetProductsAsync();
        Task<List<Product>> GetLatestProductsAsync(int count = 12);
        Task<Product?> GetProductByIdAsync(int id);
        Task<List<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<List<Product>> GetProductsByBrandAsync(int brandId);
        
        // Category methods
        Task<List<Category>> GetCategoriesAsync();
        Task<Category?> GetCategoryByIdAsync(int id);
        
        // Brand methods
        Task<List<Brand>> GetBrandsAsync();
        Task<Brand?> GetBrandByIdAsync(int id);
    }
}
