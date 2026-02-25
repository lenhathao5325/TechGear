using TechGearAPI.Models;

namespace TechGearAPI.Service.Interfaces
{
    public interface IComboService
    {
        Task<ComboAPI> CreateComboAsync(ComboAPI combo);
        Task<ComboAPI?> GetComboAsync(int comboId);
        Task<List<ComboAPI>> GetActiveCombosAsync();
        Task UpdateComboPriceAsync(int comboId);
    }
}
