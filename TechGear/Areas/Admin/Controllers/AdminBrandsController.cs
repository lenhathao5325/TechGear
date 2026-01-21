using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechGear.Service.CloudinaryService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechGear.Data;
using TechGear.Models.ViewModels;
using TechGear.Models;

namespace TechGear.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminBrandsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly Cloudinary _cloudinary;

        public AdminBrandsController(ApplicationDbContext context, Cloudinary cloudinary)
        {
            _context = context;
            _cloudinary = cloudinary;
        }

        // GET: Admin/Brands
        public async Task<IActionResult> Index()
        {
            return View(await _context.Brands
            .Include(b => b.Products) // ⭐ BẮT BUỘC
            .ToListAsync());
        }

        // GET: Admin/Brands/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.BrandId == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // GET: Admin/Brands/Create
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var brand = new Brand
            {
                BrandName = vm.BrandName,
                Description = vm.Description,
                IsActive = vm.IsActive
            };

            if (vm.LogoFile != null)
            {
                using var stream = vm.LogoFile.OpenReadStream();

                var uploadParams = new ImageUploadParams
                {
                    File = new FileDescription(vm.LogoFile.FileName, stream),
                    Folder = "techgear/brands"
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                brand.LogoUrl = uploadResult.SecureUrl.ToString();
            }

            _context.Brands.Add(brand);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }



        // GET: Admin/Brands/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var brand = await _context.Brands
                .Include(b => b.Products)
                .FirstOrDefaultAsync(b => b.BrandId == id);

            if (brand == null) return NotFound();

            // Nếu đã có product → chặn truy cập Edit
            if (brand.Products.Any())
                return RedirectToAction(nameof(Index));

            return View(brand);
        }



        // POST: Admin/Brands/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Brand model)
        {
            if (id != model.BrandId)
                return NotFound();

            bool hasProducts = await _context.Products
                .AnyAsync(p => p.BrandId == id);

            if (hasProducts)
            {
                ModelState.AddModelError("",
                    "Brand đã được dùng cho sản phẩm, không thể chỉnh sửa.");
                return View(model);
            }

            if (!ModelState.IsValid)
                return View(model);

            var brand = await _context.Brands.FindAsync(id);
            if (brand == null)
                return NotFound();

            // Chỉ update field cho phép
            brand.BrandName = model.BrandName;
            brand.Description = model.Description;
            brand.IsActive = model.IsActive;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }




        // GET: Admin/Brands/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brand = await _context.Brands
                .FirstOrDefaultAsync(m => m.BrandId == id);
            if (brand == null)
            {
                return NotFound();
            }

            return View(brand);
        }

        // POST: Admin/Brands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            bool hasProducts = await _context.Products
                .AnyAsync(p => p.BrandId == id);

            if (hasProducts)
            {
                TempData["Error"] =
                    "Brand đang được sử dụng cho sản phẩm, không thể xóa.";
                return RedirectToAction(nameof(Index));
            }

            var brand = await _context.Brands.FindAsync(id);
            if (brand != null)
            {
                _context.Brands.Remove(brand);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }


        private bool BrandExists(int id)
        {
            return _context.Brands.Any(e => e.BrandId == id);
        }
    }
}
