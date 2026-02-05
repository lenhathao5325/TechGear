using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using TechGearMVC.Models;

namespace TechGearMVC.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IHttpClientFactory _httpFactory;
        private readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

        public ProductsController(IHttpClientFactory httpFactory)
        {
            _httpFactory = httpFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpFactory.CreateClient("api");
            var res = await client.GetAsync("api/products");
            res.EnsureSuccessStatusCode();
            var json = await res.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<ProductVM>>(json, _jsonOptions);
            return View(list);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new ProductVM();
            await LoadLookups(vm);
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductVM vm)
        {
            if (!ModelState.IsValid) { await LoadLookups(vm); return View(vm); }
            var dto = new { vm.Name, vm.Description, vm.Price, vm.CategoryId, vm.BrandId };
            var json = JsonSerializer.Serialize(dto);
            var client = _httpFactory.CreateClient("api");
            var res = await client.PostAsync("api/products", new StringContent(json, Encoding.UTF8, "application/json"));
            res.EnsureSuccessStatusCode();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var client = _httpFactory.CreateClient("api");
            var res = await client.GetAsync($"api/products/{id}");
            if (!res.IsSuccessStatusCode) return NotFound();
            var json = await res.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<ProductVM>(json, _jsonOptions);
            await LoadLookups(data);
            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductVM vm)
        {
            if (!ModelState.IsValid) { await LoadLookups(vm); return View(vm); }
            var dto = new { vm.Name, vm.Description, vm.Price, vm.CategoryId, vm.BrandId };
            var json = JsonSerializer.Serialize(dto);
            var client = _httpFactory.CreateClient("api");
            var res = await client.PutAsync($"api/products/{vm.Id}", new StringContent(json, Encoding.UTF8, "application/json"));
            res.EnsureSuccessStatusCode();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var client = _httpFactory.CreateClient("api");
            var res = await client.DeleteAsync($"api/products/{id}");
            return RedirectToAction("Index");
        }

        private async Task LoadLookups(ProductVM vm)
        {
            var client = _httpFactory.CreateClient("api");
            var resCat = await client.GetAsync("api/categories");
            var resBrand = await client.GetAsync("api/brands");
            resCat.EnsureSuccessStatusCode(); resBrand.EnsureSuccessStatusCode();
            var catJson = await resCat.Content.ReadAsStringAsync();
            var brandJson = await resBrand.Content.ReadAsStringAsync();
            vm.Categories = JsonSerializer.Deserialize<List<CategoryVM>>(catJson, _jsonOptions);
            vm.Brands = JsonSerializer.Deserialize<List<BrandVM>>(brandJson, _jsonOptions);
        }
    }
}