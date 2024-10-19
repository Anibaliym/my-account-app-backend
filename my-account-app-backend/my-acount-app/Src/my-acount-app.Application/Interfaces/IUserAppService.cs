using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.User;

namespace MyAccountApp.Application.Interfaces
{
    public interface IUserAppService : IDisposable
    {
        Task<UserViewModel> GetActiveUserById(Guid id);
        Task<UserViewModel> GetActiveUserByEmail(string email);
        Task<IEnumerable<UserViewModel>> GetAllActiveUsers();
        Task<GenericResponse> RegisterUser(UserCreateViewModel model);
        Task<GenericResponse> UpdateUser(UserUpdateViewModel model);
        Task<GenericResponse> DeleteUser(Guid id);
    }
}
