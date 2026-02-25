using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using TechGear.Constants;

namespace TechGear.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AuthController(IHttpClientFactory factory, IConfiguration configuration)
        {
            _httpClientFactory = factory;
            _configuration = configuration;
        }

        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Email, string Password, string? returnUrl = null)
        {
            var data = new
            {
                email = Email,
                password = Password
            };

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            var httpClient = _httpClientFactory.CreateClient();
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"]?.Replace("/api", "");
            var res = await httpClient.PostAsync($"{apiBaseUrl}/api/auth/login", content);

            if (!res.IsSuccessStatusCode)
            {
                ViewBag.Error = "Email hoặc mật khẩu không đúng!";
                ViewBag.ReturnUrl = returnUrl;
                return View();
            }

            var responseContent = await res.Content.ReadAsStringAsync();
            var loginResponse = JObject.Parse(responseContent);

            // Lưu token và thông tin user
            var token = loginResponse["token"]?.ToString() ?? "";
            var userId = loginResponse["user"]?["id"]?.ToString() ?? "";
            var fullName = loginResponse["user"]?["fullName"]?.ToString() ?? "";
            var phoneNumber = loginResponse["user"]?["phoneNumber"]?.ToString() ?? "";
            var roles = loginResponse["user"]?["roles"] as JArray;
            
            // Get the first role (highest priority)
            var userRole = roles?.FirstOrDefault()?.ToString() ?? RoleConstants.User;

            HttpContext.Session.SetString("JWToken", token);
            HttpContext.Session.SetString("Email", Email);
            HttpContext.Session.SetString("UserId", userId);
            HttpContext.Session.SetString("FullName", fullName);
            HttpContext.Session.SetString("PhoneNumber", phoneNumber ?? "");
            HttpContext.Session.SetString("UserRole", userRole);

            // Redirect based on role
            if (userRole == RoleConstants.Admin || userRole == RoleConstants.Staff)
            {
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });
            }

            // Redirect to returnUrl if exists, otherwise go to home
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string FullName, string Email, string Password, string ConfirmPassword, string PhoneNumber)
        {
            // Validate
            if (Password != ConfirmPassword)
            {
                ViewBag.Error = "Mật khẩu xác nhận không khớp!";
                return View();
            }

            var data = new
            {
                fullName = FullName,
                email = Email,
                password = Password,
                phoneNumber = PhoneNumber
            };

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            var httpClient = _httpClientFactory.CreateClient();
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"]?.Replace("/api", "");
            var res = await httpClient.PostAsync($"{apiBaseUrl}/api/auth/register", content);

            if (!res.IsSuccessStatusCode)
            {
                var errorContent = await res.Content.ReadAsStringAsync();
                ViewBag.Error = "Đăng ký thất bại! Email có thể đã được sử dụng.";
                return View();
            }

            TempData["SuccessMessage"] = "Đăng ký thành công! Vui lòng đăng nhập.";
            return RedirectToAction(nameof(Login));
        }

        public async Task<IActionResult> Profile()
        {
            var email = HttpContext.Session.GetString("Email");
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction(nameof(Login));
            }

            // Lấy thông tin user từ API để cập nhật session
            var token = HttpContext.Session.GetString("JWToken");
            if (!string.IsNullOrEmpty(token))
            {
                var httpClient = _httpClientFactory.CreateClient();
                var apiBaseUrl = _configuration["ApiSettings:BaseUrl"]?.Replace("/api", "");
                
                var request = new HttpRequestMessage(HttpMethod.Get, $"{apiBaseUrl}/api/auth/profile");
                request.Headers.Add("Authorization", $"Bearer {token}");

                var res = await httpClient.SendAsync(request);
                if (res.IsSuccessStatusCode)
                {
                    var responseContent = await res.Content.ReadAsStringAsync();
                    var userProfile = JObject.Parse(responseContent);
                    
                    var phoneNumber = userProfile["phoneNumber"]?.ToString() ?? "";
                    HttpContext.Session.SetString("PhoneNumber", phoneNumber);
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(string FullName, string PhoneNumber)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(nameof(Login));
            }

            var data = new
            {
                fullName = FullName,
                phoneNumber = PhoneNumber
            };

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            var httpClient = _httpClientFactory.CreateClient();
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"]?.Replace("/api", "");
            
            var request = new HttpRequestMessage(HttpMethod.Put, $"{apiBaseUrl}/api/auth/profile");
            request.Headers.Add("Authorization", $"Bearer {token}");
            request.Content = content;

            var res = await httpClient.SendAsync(request);

            if (!res.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Cập nhật thông tin thất bại!";
                return RedirectToAction(nameof(Profile));
            }

            HttpContext.Session.SetString("FullName", FullName);
            HttpContext.Session.SetString("PhoneNumber", PhoneNumber ?? "");
            TempData["SuccessMessage"] = "Cập nhật thông tin thành công!";
            return RedirectToAction(nameof(Profile));
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(string CurrentPassword, string NewPassword, string ConfirmNewPassword)
        {
            var token = HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(nameof(Login));
            }

            if (NewPassword != ConfirmNewPassword)
            {
                TempData["ErrorMessage"] = "Mật khẩu mới không khớp!";
                return RedirectToAction(nameof(Profile));
            }

            var data = new
            {
                currentPassword = CurrentPassword,
                newPassword = NewPassword
            };

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");

            var httpClient = _httpClientFactory.CreateClient();
            var apiBaseUrl = _configuration["ApiSettings:BaseUrl"]?.Replace("/api", "");
            
            var request = new HttpRequestMessage(HttpMethod.Post, $"{apiBaseUrl}/api/auth/change-password");
            request.Headers.Add("Authorization", $"Bearer {token}");
            request.Content = content;

            var res = await httpClient.SendAsync(request);

            if (!res.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] = "Đổi mật khẩu thất bại! Mật khẩu hiện tại không đúng.";
                return RedirectToAction(nameof(Profile));
            }

            TempData["SuccessMessage"] = "Đổi mật khẩu thành công!";
            return RedirectToAction(nameof(Profile));
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}
