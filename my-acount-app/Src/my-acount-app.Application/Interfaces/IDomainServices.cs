using MyAccountApp.Application.Responses;

namespace MyAccountApp.Application.Interfaces
{
    public interface IDomainServices : IDisposable
    {
        Task<GenericResponse> Login(string email, string password);
    }
}
