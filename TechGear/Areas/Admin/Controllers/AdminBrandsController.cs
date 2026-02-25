using Microsoft.AspNetCore.Mvc;
using TechGear.Constants;
using TechGear.Filters;
using TechGear.Models;
using TechGear.Services;

namespace TechGear.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthorizeRole(RoleConstants.Admin, RoleConstants.Staff)]
    public class AdminBrandsController : Controller
    {
        private readonly IApiService _apiService;

        public AdminBrandsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: Admin/AdminBrands
        public async Task<IActionResult> Index()
        {
            try
            {
                var brands = await _apiService.GetAsync<List<Brand>>("api/Brands");
                return View(brands ?? new List<Brand>());
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải danh sách thương hiệu: {ex.Message}";
                return View(new List<Brand>());
            }
        }

        // GET: Admin/AdminBrands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var brand = await _apiService.GetAsync<Brand>($"api/Brands/{id}");
                if (brand == null)
                {
                    return NotFound();
                }
                return View(brand);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải thông tin thương hiệu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Admin/AdminBrands/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminBrands/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BrandName,Description,LogoUrl,IsActive")] Brand brand)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Map to DTO format expected by API
                    var brandDto = new { Name = brand.BrandName };
                    var result = await _apiService.PostAsync<Brand>("api/Brands", brandDto);
                    if (result != null)
                    {
                        TempData["Success"] = "Tạo thương hiệu thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["Error"] = "Không thể tạo thương hiệu. Vui lòng thử lại.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Lỗi khi tạo thương hiệu: {ex.Message}";
                }
            }
            return View(brand);
        }

        // GET: Admin/AdminBrands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var brand = await _apiService.GetAsync<Brand>($"api/Brands/{id}");
                if (brand == null)
                {
                    return NotFound();
                }
                return View(brand);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải thông tin thương hiệu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Admin/AdminBrands/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BrandId,BrandName,Description,LogoUrl,IsActive")] Brand brand)
        {
            if (id != brand.BrandId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Map to DTO format expected by API
                    var brandDto = new { Name = brand.BrandName };
                    var result = await _apiService.PutAsync<Brand>($"api/Brands/{id}", brandDto);
                    if (result)
                    {
                        TempData["Success"] = "Cập nhật thương hiệu thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["Error"] = "Không thể cập nhật thương hiệu. Vui lòng thử lại.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Lỗi khi cập nhật thương hiệu: {ex.Message}";
                }
            }
            return View(brand);
        }

        // GET: Admin/AdminBrands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var brand = await _apiService.GetAsync<Brand>($"api/Brands/{id}");
                if (brand == null)
                {
                    return NotFound();
                }
                return View(brand);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải thông tin thương hiệu: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Admin/AdminBrands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _apiService.DeleteAsync($"api/Brands/{id}");
                if (result)
                {
                    TempData["Success"] = "Xóa thương hiệu thành công!";
                }
                else
                {
                    TempData["Error"] = "Không thể xóa thương hiệu. Vui lòng thử lại.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi xóa thương hiệu: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
