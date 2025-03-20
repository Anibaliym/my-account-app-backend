using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.User;

namespace MyAccountApp.Application.Interfaces
{
    public interface IUserAppService : IDisposable
    {
        Task<UserViewModel> GetUserById(Guid id);
        Task<UserViewModel> GetUserByEmail(string email);
        Task<IEnumerable<UserViewModel>> GetAllUsers();
        Task<GenericResponse> RegisterUser(UserCreateViewModel model);
        Task<GenericResponse> UpdateUser(UserUpdateViewModel model);
        Task<GenericResponse> DeleteUser(Guid id);
    }
}
