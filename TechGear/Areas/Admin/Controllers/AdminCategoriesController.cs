using Microsoft.AspNetCore.Mvc;
using TechGear.Constants;
using TechGear.Filters;
using TechGear.Models;
using TechGear.Services;

namespace TechGear.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthorizeRole(RoleConstants.Admin, RoleConstants.Staff)]
    public class AdminCategoriesController : Controller
    {
        private readonly IApiService _apiService;

        public AdminCategoriesController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: Admin/AdminCategories
        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await _apiService.GetAsync<List<Category>>("api/Categories");
                return View(categories ?? new List<Category>());
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải danh sách danh mục: {ex.Message}";
                return View(new List<Category>());
            }
        }

        // GET: Admin/AdminCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var category = await _apiService.GetAsync<Category>($"api/Categories/{id}");
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải thông tin danh mục: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Admin/AdminCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AdminCategories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryName,Description,ImageUrl,IsActive")] Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Map to DTO format expected by API
                    var categoryDto = new { Name = category.CategoryName };
                    var result = await _apiService.PostAsync<Category>("api/Categories", categoryDto);
                    if (result != null)
                    {
                        TempData["Success"] = "Tạo danh mục thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["Error"] = "Không thể tạo danh mục. Vui lòng thử lại.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Lỗi khi tạo danh mục: {ex.Message}";
                }
            }
            return View(category);
        }

        // GET: Admin/AdminCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var category = await _apiService.GetAsync<Category>($"api/Categories/{id}");
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải thông tin danh mục: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Admin/AdminCategories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,Description,ImageUrl,IsActive")] Category category)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Map to DTO format expected by API
                    var categoryDto = new { Name = category.CategoryName };
                    var result = await _apiService.PutAsync<Category>($"api/Categories/{id}", categoryDto);
                    if (result)
                    {
                        TempData["Success"] = "Cập nhật danh mục thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["Error"] = "Không thể cập nhật danh mục. Vui lòng thử lại.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Lỗi khi cập nhật danh mục: {ex.Message}";
                }
            }
            return View(category);
        }

        // GET: Admin/AdminCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var category = await _apiService.GetAsync<Category>($"api/Categories/{id}");
                if (category == null)
                {
                    return NotFound();
                }
                return View(category);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải thông tin danh mục: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Admin/AdminCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _apiService.DeleteAsync($"api/Categories/{id}");
                if (result)
                {
                    TempData["Success"] = "Xóa danh mục thành công!";
                }
                else
                {
                    TempData["Error"] = "Không thể xóa danh mục. Vui lòng thử lại.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi xóa danh mục: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
