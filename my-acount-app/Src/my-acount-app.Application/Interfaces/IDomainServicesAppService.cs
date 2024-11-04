using MyAccountApp.Application.Responses;

namespace MyAccountApp.Application.Interfaces
{
    public interface IDomainServicesAppService : IDisposable
    {
        Task<GenericResponse> Login(string email, string password);
        Task<GenericResponse> GetSheetsAccount(Guid accountId);
    }
}
