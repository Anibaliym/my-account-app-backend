using MyAccountApp.Application.Responses;

namespace MyAccountApp.Application.Interfaces
{
    public interface IDomainServicesAppService : IDisposable
    {
        Task<GenericResponse> Login(string email, string password);
        Task<GenericResponse> GetSheetsAccount(Guid accountId);
        Task<GenericResponse> GetSheetCardsWithVignettes(Guid sheetId);
        Task<GenericResponse> GetUserAccountsWithSheets(Guid userId);
        Task<GenericResponse> DeleteCardWithVignettes(Guid cardId);
    }
}
