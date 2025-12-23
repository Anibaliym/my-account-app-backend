using Microsoft.EntityFrameworkCore;
using MyAccountApp.Core.Entities;
using MyAccountApp.Core.Interfaces;
using MyAccountApp.Infrastructure.Context;

namespace MyAccountApp.Infrastructure.Repositories
{
    public class UserAccessLogRepository : IUserAccessLogRepository
    {
        private readonly my_account_appAppDbContext _dbContext; 

        public UserAccessLogRepository(my_account_appAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserAccessLog?> GetUserAccessLogById(Guid id)
        {
            return await _dbContext.UserAccessLog.Where(userAccessLog => userAccessLog.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserAccessLog>> GetAllUserAccessLogByUserId(Guid userId)
        {
            return await _dbContext.UserAccessLog.AsNoTracking().Where(userAccessLog => userAccessLog.UserId == userId).ToListAsync();   
        }

        // public async Task<IEnumerable<UserAccessLog>> GetAllSuccessUserAccessLogByUserId(Guid userId)
        // {
        //     return await _dbContext.UserAccessLog.AsNoTracking()
        //         .Where(userAccessLog => userAccessLog.UserId == userId && userAccessLog.Success == true)
        //         .OrderBy(userAccessLog => userAccessLog.OccurredAt)
        //         .ToListAsync();   
        // }

        public async Task<IEnumerable<UserAccessLog>> GetAllSuccessUserAccessLogByUserId(Guid userId)
        {
            return await _dbContext.UserAccessLog
                .AsNoTracking()
                .Where(log => log.UserId == userId && log.Success)
                .OrderByDescending(log => log.OccurredAt)
                .Take(5)
                .ToListAsync();
        }

        public async Task RegisterAccessUserLog(UserAccessLog model)
        {
            _dbContext.UserAccessLog.Add(model);
            await _dbContext.SaveChangesAsync();                
        }
    }
}