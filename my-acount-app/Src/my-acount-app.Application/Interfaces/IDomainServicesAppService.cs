using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.Vignette;

namespace MyAccountApp.Application.Interfaces
{
    public interface IDomainServicesAppService : IDisposable
    {
        Task<GenericResponse> Login(string email, string password);
        Task<GenericResponse> GetSheetsAccount(Guid accountId);
        Task<GenericResponse> GetSheetCardsWithVignettes(Guid sheetId);
        Task<GenericResponse> GetUserAccountsWithSheets(Guid userId);
        Task<GenericResponse> DeleteCardWithVignettes(Guid cardId);
        Task<GenericResponse> DeleteSheetWithContents(Guid sheetId);
        Task<GenericResponse> UpdateVignetteAndRecalculateTotal(VignetteViewModel model);
        Task<GenericResponse> UpdateVignetteColorTheme(Guid vignetteId, string colorTheme);
        Task<GenericResponse> DeleteVignetteAndRecalculateTotal(Guid vignetteId);
        Task<GenericResponse> CreateSheetBackup(Guid sheetId);
    }
}
