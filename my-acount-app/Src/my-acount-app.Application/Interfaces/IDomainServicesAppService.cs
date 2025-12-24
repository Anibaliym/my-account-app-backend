using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.Vignette;

namespace MyAccountApp.Application.Interfaces
{
    public interface IDomainServicesAppService : IDisposable
    {
        Task<GenericResponse> Login(string email, string password, string ip, string userAgent);
        Task<GenericResponse> GetSheetsAccount(Guid accountId);
        Task<GenericResponse> GetSheetCardsWithVignettes(Guid sheetId);
        Task<GenericResponse> GetUserAccountsWithSheets(Guid userId);
        Task<GenericResponse> GetAllSuccessUserAccessLogByUserId(Guid userId);
        Task<GenericResponse> DeleteCardWithVignettes(Guid cardId);
        Task<GenericResponse> DeleteUserAccount(DeleteUserRequest request);
        Task<GenericResponse> DeleteSheetWithContents(Guid sheetId);
        Task<GenericResponse> UpdateVignetteAndRecalculateTotal(VignetteViewModel model);
        Task<GenericResponse> UpdateVignetteColorTheme(Guid vignetteId, string colorTheme);
        Task<GenericResponse> DeleteVignetteAndRecalculateTotal(Guid vignetteId);
        Task<GenericResponse> CreateSheetBackup(Guid sheetId);
    }
}
