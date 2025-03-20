using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.Account;

namespace MyAccountApp.Application.Interfaces
{
    public interface IAccountAppService : IDisposable
    {
        Task<AccountViewModel> GetAccountById(Guid id);
        Task<IEnumerable<AccountViewModel>> GetAccountByUserId(Guid userId);
        Task<GenericResponse> CreateAccount(CreateAccountViewModel model);
        Task<GenericResponse> UpdateAccount(UpdateAccountViewModel model);
        Task<GenericResponse> UpdateAccountOrderItems(List<UpdateAccountViewModel> model);
        Task<GenericResponse> DeleteAccount(Guid id);
    }
}
