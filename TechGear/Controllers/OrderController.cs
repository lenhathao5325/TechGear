using Microsoft.AspNetCore.Mvc;
using TechGear.Filters;
using TechGear.Services;
using Newtonsoft.Json;
using System.Text;

namespace TechGear.Controllers
{
    [RequireLogin]
    public class OrderController : Controller
    {
        private readonly IApiService _apiService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public OrderController(IApiService apiService, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _apiService = apiService;
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<IActionResult> Checkout()
        {
            var token = HttpContext.Session.GetString("JWToken");

            // Get cart items
            var httpClient = _httpClientFactory.CreateClient();
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"]?.Replace("/api", "");

            var cartRequest = new HttpRequestMessage(HttpMethod.Get, $"{apiBaseUrl}/api/cart");
            cartRequest.Headers.Add("Authorization", $"Bearer {token}");

            var cartResponse = await httpClient.SendAsync(cartRequest);

            if (!cartResponse.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Không thể tải giỏ hàng!";
                return RedirectToAction("Index", "Cart");
            }

            var cartContent = await cartResponse.Content.ReadAsStringAsync();
            var cartItems = JsonConvert.DeserializeObject<List<Models.CartItem>>(cartContent);

            // Get user addresses
            var addressRequest = new HttpRequestMessage(HttpMethod.Get, $"{apiBaseUrl}/api/useraddress");
            addressRequest.Headers.Add("Authorization", $"Bearer {token}");

            var addressResponse = await httpClient.SendAsync(addressRequest);

            if (addressResponse.IsSuccessStatusCode)
            {
                var addressContent = await addressResponse.Content.ReadAsStringAsync();
                var addresses = JsonConvert.DeserializeObject<List<Models.UserAddress>>(addressContent);
                ViewBag.Addresses = addresses;
            }

            ViewBag.CartItems = cartItems;
            ViewBag.SubTotal = cartItems?.Sum(x => x.TotalPrice) ?? 0;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder(int userAddressId, string paymentMethod = "COD")
        {
            var token = HttpContext.Session.GetString("JWToken");

            var data = new
            {
                userAddressId = userAddressId,
                paymentMethod = paymentMethod
            };

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var httpClient = _httpClientFactory.CreateClient();
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"]?.Replace("/api", "");

            var request = new HttpRequestMessage(HttpMethod.Post, $"{apiBaseUrl}/api/orders");
            request.Headers.Add("Authorization", $"Bearer {token}");
            request.Content = content;

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var orderResponse = JsonConvert.DeserializeObject<dynamic>(responseContent);
                int orderId = orderResponse.orderId;

                TempData["SuccessMessage"] = "Đặt hàng thành công!";
                return RedirectToAction(nameof(OrderConfirmation), new { id = orderId });
            }

            TempData["ErrorMessage"] = "Đặt hàng thất bại!";
            return RedirectToAction(nameof(Checkout));
        }

        public async Task<IActionResult> OrderConfirmation(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");

            var httpClient = _httpClientFactory.CreateClient();
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"]?.Replace("/api", "");

            var request = new HttpRequestMessage(HttpMethod.Get, $"{apiBaseUrl}/api/orders/{id}");
            request.Headers.Add("Authorization", $"Bearer {token}");

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<Models.Order>(content);
                return View(order);
            }

            return RedirectToAction(nameof(MyOrders));
        }

        public async Task<IActionResult> MyOrders()
        {
            var token = HttpContext.Session.GetString("JWToken");

            var httpClient = _httpClientFactory.CreateClient();
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"]?.Replace("/api", "");

            var request = new HttpRequestMessage(HttpMethod.Get, $"{apiBaseUrl}/api/orders/my-orders");
            request.Headers.Add("Authorization", $"Bearer {token}");

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<List<Models.Order>>(content);
                return View(orders);
            }

            return View(new List<Models.Order>());
        }

        public async Task<IActionResult> OrderDetails(int id)
        {
            var token = HttpContext.Session.GetString("JWToken");

            var httpClient = _httpClientFactory.CreateClient();
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"]?.Replace("/api", "");

            var request = new HttpRequestMessage(HttpMethod.Get, $"{apiBaseUrl}/api/orders/{id}");
            request.Headers.Add("Authorization", $"Bearer {token}");

            var response = await httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var order = JsonConvert.DeserializeObject<Models.Order>(content);
                return View(order);
            }

            return RedirectToAction(nameof(MyOrders));
        }
    }
}
