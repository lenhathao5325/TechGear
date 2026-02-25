using System.Text;
using System.Text.Json;
using TechGear.Models;

namespace TechGear.Services
{
    public class ApiService : IApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ApiService> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiService(IHttpClientFactory httpClientFactory, ILogger<ApiService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        private HttpClient CreateClient()
        {
            return _httpClientFactory.CreateClient("TechGearAPI");
        }

        // Generic GET method
        public async Task<T?> GetAsync<T>(string endpoint)
        {
            try
            {
                var client = CreateClient();
                var response = await client.GetAsync(endpoint);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(content, _jsonOptions);
                }
                else
                {
                    _logger.LogError($"API GET call to {endpoint} failed with status code: {response.StatusCode}");
                    return default;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling GET API {endpoint}: {ex.Message}");
                return default;
            }
        }

        // Generic POST method
        public async Task<T?> PostAsync<T>(string endpoint, object data)
        {
            try
            {
                var client = CreateClient();
                var json = JsonSerializer.Serialize(data, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await client.PostAsync(endpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(responseContent, _jsonOptions);
                }
                else
                {
                    _logger.LogError($"API POST call to {endpoint} failed with status code: {response.StatusCode}");
                    return default;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling POST API {endpoint}: {ex.Message}");
                return default;
            }
        }

        // Generic PUT method
        public async Task<bool> PutAsync<T>(string endpoint, object data)
        {
            try
            {
                var client = CreateClient();
                var json = JsonSerializer.Serialize(data, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                
                var response = await client.PutAsync(endpoint, content);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"API PUT call to {endpoint} failed with status code: {response.StatusCode}");
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling PUT API {endpoint}: {ex.Message}");
                return false;
            }
        }

        // Generic DELETE method
        public async Task<bool> DeleteAsync(string endpoint)
        {
            try
            {
                var client = CreateClient();
                var response = await client.DeleteAsync(endpoint);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"API DELETE call to {endpoint} failed with status code: {response.StatusCode}");
                }
                
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling DELETE API {endpoint}: {ex.Message}");
                return false;
            }
        }

        // Specific methods for Product
        public async Task<List<Product>> GetProductsAsync()
        {
            return await GetAsync<List<Product>>("api/products") ?? new List<Product>();
        }

        public async Task<List<Product>> GetLatestProductsAsync(int count = 12)
        {
            return await GetAsync<List<Product>>($"api/products/latest/{count}") ?? new List<Product>();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await GetAsync<Product>($"api/products/{id}");
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await GetAsync<List<Product>>($"api/products/category/{categoryId}") ?? new List<Product>();
        }

        public async Task<List<Product>> GetProductsByBrandAsync(int brandId)
        {
            return await GetAsync<List<Product>>($"api/products/brand/{brandId}") ?? new List<Product>();
        }

        // Specific methods for Category
        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await GetAsync<List<Category>>("api/categories") ?? new List<Category>();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await GetAsync<Category>($"api/categories/{id}");
        }

        // Specific methods for Brand
        public async Task<List<Brand>> GetBrandsAsync()
        {
            return await GetAsync<List<Brand>>("api/brands") ?? new List<Brand>();
        }

        public async Task<Brand?> GetBrandByIdAsync(int id)
        {
            return await GetAsync<Brand>($"api/brands/{id}");
        }
    }
}
