using MyAccountApp.Core.Entities;

namespace MyAccountApp.Core.Interfaces
{
    public interface ISheetRepository
    {
        Task<Sheet> GetSheetById(Guid id);
        Task<IEnumerable<Sheet>> GetSheetByAccountId(Guid acoountId); 
        Task CreateSheet(Sheet model);
        Task UpdateSheet(Sheet model);
        Task<bool> DeleteSheet(Guid id);
    }
}
