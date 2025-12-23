using MyAccountApp.Core.Entities;

namespace MyAccountApp.Core.Interfaces
{
    public interface IUserAccessLogRepository
    {
        Task<UserAccessLog> GetUserAccessLogById(Guid id);
        Task<IEnumerable<UserAccessLog>> GetAllUserAccessLogByUserId(Guid userId);
        Task<IEnumerable<UserAccessLog>> GetAllSuccessUserAccessLogByUserId(Guid userId);
        Task RegisterAccessUserLog(UserAccessLog model); 
    }
}