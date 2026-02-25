using TechGearAPI.Service.Helpers;
using Microsoft.EntityFrameworkCore;
using TechGearAPI.Data;
using TechGearAPI.Service.Interfaces;
using TechGearAPI.Models;

namespace TechGearAPI.Service.Implementations
{
    public class ComboService : IComboService
    {
        private readonly ApplicationDbContext _context;

        public ComboService(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Tạo combo mới
        public async Task<ComboAPI> CreateComboAsync(ComboAPI combo)
        {
            combo.OriginalPrice = combo.ComboItems.Sum(
                x => x.ProductVariant.Price * x.Quantity
            );

            combo.FinalPrice = ComboPriceCalculator.Calculate(
                combo.OriginalPrice,
                combo.DiscountType,
                combo.DiscountValue
            );

            _context.comboAPIs.Add(combo);
            await _context.SaveChangesAsync();

            return combo;
        }

        // ✅ Lấy combo theo id
        public async Task<ComboAPI?> GetComboAsync(int comboId)
        {
            return await _context.comboAPIs
                .Include(c => c.ComboItems)
                    .ThenInclude(ci => ci.ProductVariant)
                .FirstOrDefaultAsync(c => c.ComboId == comboId && c.IsActive);
        }

        // ✅ Danh sách combo đang bán
        public async Task<List<ComboAPI>> GetActiveCombosAsync()
        {
            return await _context.comboAPIs
                .Where(c => c.IsActive)
                .Include(c => c.ComboItems)
                .ToListAsync();
        }

        // ✅ Update lại giá khi admin chỉnh sản phẩm
        public async Task UpdateComboPriceAsync(int comboId)
        {
            var combo = await _context.comboAPIs
                .Include(c => c.ComboItems)
                    .ThenInclude(ci => ci.ProductVariant)
                .FirstOrDefaultAsync(c => c.ComboId == comboId);

            if (combo == null) return;

            combo.OriginalPrice = combo.ComboItems.Sum(
                x => x.ProductVariant.Price * x.Quantity
            );

            combo.FinalPrice = ComboPriceCalculator.Calculate(
                combo.OriginalPrice,
                combo.DiscountType,
                combo.DiscountValue
            );

            await _context.SaveChangesAsync();
        }
    }
}
