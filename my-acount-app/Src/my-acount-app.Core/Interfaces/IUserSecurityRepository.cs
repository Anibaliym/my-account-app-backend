using MyAccountApp.Core.Entities;

namespace MyAccountApp.Core.Interfaces
{
    public interface IUserSecurityRepository
    {
        Task<UserSecurity> GetUserSecurityById(Guid id);
        Task<UserSecurity> GetUserSecurityByUserId(Guid userId);
        Task CreateUserSecurity(UserSecurity userSecurity);
        Task UpdateUserSecurity(UserSecurity userSecurity);
        Task<bool> DeleteUserSecurity(Guid id);
    }
}
