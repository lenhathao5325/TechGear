using Microsoft.AspNetCore.Mvc;
using TechGear.Filters;
using TechGear.Services;
using Newtonsoft.Json;
using System.Text;

namespace TechGear.Controllers
{
    public class CartController : Controller
    {
        private readonly IApiService _apiService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public CartController(IApiService apiService, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _apiService = apiService;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [RequireLogin]
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("JWToken");
            var userId = HttpContext.Session.GetString("UserId");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            // TODO: Call API to get cart items
            var httpClient = _httpClientFactory.CreateClient();
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"]?.Replace("/api", "");

            var request = new HttpRequestMessage(HttpMethod.Get, $"{apiBaseUrl}/api/cart");
            request.Headers.Add("Authorization", $"Bearer {token}");

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var cartItems = JsonConvert.DeserializeObject<List<Models.CartItem>>(content);
                return View(cartItems);
            }

            return View(new List<Models.CartItem>());
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productVariantId, int quantity = 1)
        {
            var token = HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth", new { returnUrl = Request.Headers["Referer"].ToString() });
            }

            var data = new
            {
                productVariantId = productVariantId,
                quantity = quantity
            };

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var httpClient = _httpClientFactory.CreateClient();
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"]?.Replace("/api", "");

            var request = new HttpRequestMessage(HttpMethod.Post, $"{apiBaseUrl}/api/cart/add");
            request.Headers.Add("Authorization", $"Bearer {token}");
            request.Content = content;

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Đã thêm sản phẩm vào giỏ hàng!";
            }
            else
            {
                TempData["ErrorMessage"] = "Không thể thêm sản phẩm vào giỏ hàng!";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [RequireLogin]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            var token = HttpContext.Session.GetString("JWToken");

            var data = new { quantity = quantity };
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var httpClient = _httpClientFactory.CreateClient();
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"]?.Replace("/api", "");

            var request = new HttpRequestMessage(HttpMethod.Put, $"{apiBaseUrl}/api/cart/{cartItemId}");
            request.Headers.Add("Authorization", $"Bearer {token}");
            request.Content = content;

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Đã cập nhật số lượng!";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [RequireLogin]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            var token = HttpContext.Session.GetString("JWToken");

            var httpClient = _httpClientFactory.CreateClient();
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"]?.Replace("/api", "");

            var request = new HttpRequestMessage(HttpMethod.Delete, $"{apiBaseUrl}/api/cart/{cartItemId}");
            request.Headers.Add("Authorization", $"Bearer {token}");

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Đã xóa sản phẩm khỏi giỏ hàng!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
