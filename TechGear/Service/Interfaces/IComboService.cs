using TechGear.Models;

namespace TechGear.Services.Interfaces
{
    public interface IComboService
    {
        Task<Combo> CreateComboAsync(Combo combo);
        Task<Combo?> GetComboAsync(int comboId);
        Task<List<Combo>> GetActiveCombosAsync();
        Task UpdateComboPriceAsync(int comboId);
    }
}
