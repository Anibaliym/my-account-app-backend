using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.Sheet;

namespace MyAccountApp.Application.Interfaces
{
    public interface ISheetAppService : IDisposable
    {
        Task<SheetViewModel> GetSheetById(Guid id);
        Task<SheetViewModel> GetSheetAccountByOrder(int order, Guid accountId);
        Task<IEnumerable<SheetViewModel>> GetSheetByAccountId(Guid accountId);
        Task<GenericResponse> CreateSheet(CreateSheetViewModel model);
        Task<GenericResponse> UpdateSheet(UpdateSheetViewModel model);
        Task<GenericResponse> UpdateCashBalance(Guid sheetId, int newCashBalance);
        Task<GenericResponse> UpdateCurrenteAccountBalance(Guid sheetId, int currentAccountBalance);
        Task<GenericResponse> UpdateSheetOrderItems(List<UpdateSheetViewModel> model);
        Task<GenericResponse> DeleteSheet(Guid id);
    }
}
