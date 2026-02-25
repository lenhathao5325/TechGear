using Microsoft.AspNetCore.Mvc;
using TechGear.Services;

namespace TechGear.ViewComponents
{
    public class CategoryBarViewComponent : ViewComponent
    {
        private readonly IApiService _apiService;
        private readonly ILogger<CategoryBarViewComponent> _logger;

        public CategoryBarViewComponent(IApiService apiService, ILogger<CategoryBarViewComponent> logger)
        {
            _apiService = apiService;
            _logger = logger;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var categories = await _apiService.GetCategoriesAsync();
                return View(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading categories: {ex.Message}");
                return View(new List<TechGear.Models.Category>());
            }
        }
    }
}
