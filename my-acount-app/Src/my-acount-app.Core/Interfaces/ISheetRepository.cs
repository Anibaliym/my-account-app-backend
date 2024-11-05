using MyAccountApp.Core.Entities;

namespace MyAccountApp.Core.Interfaces
{
    public interface ISheetRepository
    {
        Task<Sheet> GetSheetById(Guid id);
        Task<Sheet> GetSheetAccountByOrder(int order, Guid accountId);
        Task<int> GetNextOrderByAccountId(Guid accountId); 
        Task<IEnumerable<Sheet>> GetSheetByAccountId(Guid acoountId); 
        Task CreateSheet(Sheet model);
        Task UpdateSheet(Sheet model);
        Task<bool> DeleteSheet(Guid id);
    }
}
