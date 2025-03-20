using MyAccountApp.Core.Entities;

namespace MyAccountApp.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetUserById(Guid id);
        Task<User> GetUserByEmail(string email);
        Task<List<User>> GetAllUsers();
        Task CreateUser(User user);
        Task UpdateUser(User user);
        Task<bool> DeleteUser(Guid id);
    }
}