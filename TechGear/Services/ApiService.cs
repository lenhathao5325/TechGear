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

        private async Task<T?> GetAsync<T>(string endpoint)
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
                    _logger.LogError($"API call to {endpoint} failed with status code: {response.StatusCode}");
                    return default;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error calling API {endpoint}: {ex.Message}");
                return default;
            }
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            return await GetAsync<List<Product>>("/ProductAPIs") ?? new List<Product>();
        }

        public async Task<List<Product>> GetLatestProductsAsync(int count = 12)
        {
            return await GetAsync<List<Product>>($"/ProductAPIs/latest/{count}") ?? new List<Product>();
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            return await GetAsync<Product>($"/ProductAPIs/{id}");
        }

        public async Task<List<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await GetAsync<List<Product>>($"/ProductAPIs/category/{categoryId}") ?? new List<Product>();
        }

        public async Task<List<Product>> GetProductsByBrandAsync(int brandId)
        {
            return await GetAsync<List<Product>>($"/ProductAPIs/brand/{brandId}") ?? new List<Product>();
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await GetAsync<List<Category>>("/CategoryAPIs") ?? new List<Category>();
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await GetAsync<Category>($"/CategoryAPIs/{id}");
        }

        public async Task<List<Brand>> GetBrandsAsync()
        {
            return await GetAsync<List<Brand>>("/BrandAPIs") ?? new List<Brand>();
        }

        public async Task<Brand?> GetBrandByIdAsync(int id)
        {
            return await GetAsync<Brand>($"/BrandAPIs/{id}");
        }
    }
}
