using MyAccountApp.Core.Entities;

namespace MyAccountApp.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetActiveUserById(Guid id);
        Task<User> GetActiveUserByEmail(string email);
        Task<List<User>> GetAllActiveUsers();
        Task<List<User>> GetAllInactiveUsers();
        Task CreateUser(User user);
        Task UpdateUser(User user);
        Task<bool> DeleteUser(Guid id);
    }
}