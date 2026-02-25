using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechGearAPI.Data;
using TechGearAPI.DTO;
using TechGearAPI.Models;
using static TechGearAPI.Models.Enums.EnumsAPI;

namespace TechGearAPI.Controllers
{
    [Route("api/combos")]
    [ApiController]
    public class ComboAPIsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ComboAPIsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ================= GET ALL =================
        [HttpGet]
        public IActionResult GetCombos()
        {
            var data = _context.comboAPIs
                .Select(c => new
                {
                    c.ComboId,
                    c.ComboName,
                    c.Description,
                    c.OriginalPrice,
                    c.FinalPrice,
                    c.DiscountType,
                    c.DiscountValue,
                    c.IsActive,
                    c.CreatedAt
                })
                .ToList();

            return Ok(data);
        }

        // ================= GET BY ID =================
        [HttpGet("{id}")]
        public IActionResult GetCombo(int id)
        {
            var combo = _context.comboAPIs
                .Where(c => c.ComboId == id)
                .Select(c => new
                {
                    c.ComboId,
                    c.ComboName,
                    c.Description,
                    c.OriginalPrice,
                    c.FinalPrice,
                    c.DiscountType,
                    c.DiscountValue,
                    c.IsActive,
                    c.CreatedAt
                })
                .FirstOrDefault();

            if (combo == null)
                return NotFound();

            return Ok(combo);
        }

        // ================= CREATE =================
        [HttpPost]
        public IActionResult Create(ComboDTO dto)
        {
            var finalPrice = CalculateFinalPrice(
                dto.OriginalPrice,
                dto.DiscountType,
                dto.DiscountValue
            );

            var combo = new ComboAPI
            {
                ComboName = dto.ComboName,
                Description = dto.Description,
                OriginalPrice = dto.OriginalPrice,
                DiscountType = dto.DiscountType,
                DiscountValue = dto.DiscountValue,
                FinalPrice = finalPrice,
                IsActive = dto.IsActive,
                CreatedAt = dto.CreatedAt
            };

            _context.comboAPIs.Add(combo);
            _context.SaveChanges();

            return Ok(new
            {
                message = "Tạo combo thành công",
                combo
            });
        }

        // ================= UPDATE =================
        [HttpPut("{id}")]
        public IActionResult Update(int id, ComboDTO dto)
        {
            var combo = _context.comboAPIs.FirstOrDefault(c => c.ComboId == id);

            if (combo == null)
                return NotFound();

            combo.ComboName = dto.ComboName;
            combo.Description = dto.Description;
            combo.OriginalPrice = dto.OriginalPrice;
            combo.DiscountType = dto.DiscountType;
            combo.DiscountValue = dto.DiscountValue;
            combo.IsActive = dto.IsActive;

            combo.FinalPrice = CalculateFinalPrice(
                dto.OriginalPrice,
                dto.DiscountType,
                dto.DiscountValue
            );

            _context.SaveChanges();

            return Ok(new
            {
                message = "Cập nhật combo thành công",
                combo
            });
        }

        // ================= DELETE =================
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var combo = _context.comboAPIs.FirstOrDefault(c => c.ComboId == id);

            if (combo == null)
                return NotFound();

            _context.comboAPIs.Remove(combo);
            _context.SaveChanges();

            return Ok(new { message = "Đã xoá combo" });
        }

        // ================= PRICE HELPER =================
        private decimal CalculateFinalPrice(decimal original, DiscountType type, decimal value)
        {
            return type switch
            {
                DiscountType.Percentage => original - (original * value / 100),
                DiscountType.FixedAmount => original - value,
                _ => original
            };
        }
    }
}
