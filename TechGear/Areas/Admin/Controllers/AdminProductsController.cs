using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechGear.Constants;
using TechGear.Filters;
using TechGear.Models;
using TechGear.Models.ViewModels;
using TechGear.Services;

namespace TechGear.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AuthorizeRole(RoleConstants.Admin, RoleConstants.Staff)]
    public class AdminProductsController : Controller
    {
        private readonly IApiService _apiService;

        public AdminProductsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: Admin/AdminProducts
        public async Task<IActionResult> Index()
        {
            try
            {
                var products = await _apiService.GetAsync<List<Product>>("api/products");
                return View(products ?? new List<Product>());
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải danh sách sản phẩm: {ex.Message}";
                return View(new List<Product>());
            }
        }

        // GET: Admin/AdminProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var product = await _apiService.GetAsync<Product>($"api/products/{id}");
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải thông tin sản phẩm: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Admin/AdminProducts/Create
        public async Task<IActionResult> Create()
        {
            var viewModel = new ProductVM();
            await LoadDropdownData(viewModel);
            return View(viewModel);
        }

        // POST: Admin/AdminProducts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductVM viewModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string imageUrl = null;

                    // Upload image if provided
                    if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
                    {
                        try
                        {
                            // Create multipart form data
                            using var formData = new MultipartFormDataContent();
                            using var streamContent = new StreamContent(viewModel.ImageFile.OpenReadStream());
                            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(viewModel.ImageFile.ContentType);
                            formData.Add(streamContent, "file", viewModel.ImageFile.FileName);

                            // Call upload API (Create)
                            var uploadResponse = await _apiService.PostMultipartAsync<System.Text.Json.JsonElement>("api/upload/image?folder=products", formData);

                            if (uploadResponse.ValueKind != System.Text.Json.JsonValueKind.Undefined
                                && uploadResponse.TryGetProperty("success", out var successPropC)
                                && successPropC.GetBoolean()
                                && uploadResponse.TryGetProperty("imageUrl", out var urlPropC))
                            {
                                imageUrl = urlPropC.GetString();
                            }
                        }
                        catch (Exception uploadEx)
                        {
                            TempData["Error"] = $"Lỗi khi upload ảnh: {uploadEx.Message}";
                            await LoadDropdownData(viewModel);
                            return View(viewModel);
                        }
                    }

                    // Map to DTO format expected by API
                    var productDto = new 
                    { 
                        Name = viewModel.Name,
                        Description = viewModel.Description,
                        CategoryId = viewModel.CategoryId,
                        BrandId = viewModel.BrandId,
                        ImageUrl = imageUrl
                    };
                    
                    var result = await _apiService.PostAsync<Product>("api/products", productDto);
                    if (result != null)
                    {
                        TempData["Success"] = "Tạo sản phẩm thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["Error"] = "Không thể tạo sản phẩm. Vui lòng thử lại.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Lỗi khi tạo sản phẩm: {ex.Message}";
                }
            }
            await LoadDropdownData(viewModel);
            return View(viewModel);
        }

        // GET: Admin/AdminProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var product = await _apiService.GetAsync<Product>($"api/products/{id}");
                if (product == null)
                {
                    return NotFound();
                }
                
                var viewModel = new ProductVM
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    CategoryId = product.CategoryId.HasValue ? product.CategoryId.Value : 0,
                    BrandId = product.BrandId.HasValue ? product.BrandId.Value : 0,
                    ImageUrl = product.ImageUrl
                };
                
                await LoadDropdownData(viewModel);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải thông tin sản phẩm: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Admin/AdminProducts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductVM viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string imageUrl = viewModel.ImageUrl; // Keep existing image by default

                    // Upload new image if provided
                    if (viewModel.ImageFile != null && viewModel.ImageFile.Length > 0)
                    {
                        try
                        {
                            // Create multipart form data
                            using var formData = new MultipartFormDataContent();
                            using var streamContent = new StreamContent(viewModel.ImageFile.OpenReadStream());
                            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(viewModel.ImageFile.ContentType);
                            formData.Add(streamContent, "file", viewModel.ImageFile.FileName);

                            // Call upload API (Edit)
                            var uploadResponse = await _apiService.PostMultipartAsync<System.Text.Json.JsonElement>("api/upload/image?folder=products", formData);

                            if (uploadResponse.ValueKind != System.Text.Json.JsonValueKind.Undefined
                                && uploadResponse.TryGetProperty("success", out var successPropE)
                                && successPropE.GetBoolean()
                                && uploadResponse.TryGetProperty("imageUrl", out var urlPropE))
                            {
                                imageUrl = urlPropE.GetString();
                            }
                        }
                        catch (Exception uploadEx)
                        {
                            TempData["Error"] = $"Lỗi khi upload ảnh: {uploadEx.Message}";
                            await LoadDropdownData(viewModel);
                            return View(viewModel);
                        }
                    }

                    // Map to DTO format expected by API
                    var productDto = new 
                    { 
                        Name = viewModel.Name,
                        Description = viewModel.Description,
                        CategoryId = viewModel.CategoryId,
                        BrandId = viewModel.BrandId,
                        ImageUrl = imageUrl
                    };
                    
                    var result = await _apiService.PutAsync<Product>($"api/products/{id}", productDto);
                    if (result)
                    {
                        TempData["Success"] = "Cập nhật sản phẩm thành công!";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["Error"] = "Không thể cập nhật sản phẩm. Vui lòng thử lại.";
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = $"Lỗi khi cập nhật sản phẩm: {ex.Message}";
                }
            }
            await LoadDropdownData(viewModel);
            return View(viewModel);
        }

        // GET: Admin/AdminProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            try
            {
                var product = await _apiService.GetAsync<Product>($"api/products/{id}");
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải thông tin sản phẩm: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Admin/AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var result = await _apiService.DeleteAsync($"api/products/{id}");
                if (result)
                {
                    TempData["Success"] = "Xóa sản phẩm thành công!";
                }
                else
                {
                    TempData["Error"] = "Không thể xóa sản phẩm. Vui lòng thử lại.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi xóa sản phẩm: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadDropdownData(ProductVM viewModel)
        {
            try
            {
                var categories = await _apiService.GetAsync<List<Category>>("api/Categories");
                var brands = await _apiService.GetAsync<List<Brand>>("api/Brands");

                viewModel.Categories = new SelectList(categories ?? new List<Category>(), "CategoryId", "CategoryName");
                viewModel.Brands = new SelectList(brands ?? new List<Brand>(), "BrandId", "BrandName");
            }
            catch (Exception ex)
            {
                viewModel.Categories = new SelectList(new List<Category>(), "CategoryId", "CategoryName");
                viewModel.Brands = new SelectList(new List<Brand>(), "BrandId", "BrandName");
                TempData["Error"] = $"Lỗi khi tải dữ liệu dropdown: {ex.Message}";
            }
        }
    }
}
