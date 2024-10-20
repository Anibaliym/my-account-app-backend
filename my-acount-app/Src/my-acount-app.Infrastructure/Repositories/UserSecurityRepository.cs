using FluentValidation;
using Microsoft.EntityFrameworkCore;
using MyAccountApp.Core.Entities;
using MyAccountApp.Core.Interfaces;
using MyAccountApp.Infrastructure.Context;

namespace MyAccountApp.Infrastructure.Repositories
{
    public class UserSecurityRepository : IUserSecurityRepository
    {
        private readonly my_account_appAppDbContext _dbContext;

        public UserSecurityRepository(my_account_appAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task CreateUserSecurity(UserSecurity userSecurity)
        {
            await _dbContext.UserSecurity.AddAsync(userSecurity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteUserSecurity(Guid id)
        {
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<UserSecurity> GetUserSecurityById(Guid id)
        {
            return await _dbContext.UserSecurity.AsNoTracking().Where(userSecurity => userSecurity.Id == id).FirstOrDefaultAsync();
        }

        public async Task<UserSecurity> GetUserSecurityByUserId(Guid userId)
        {
            return await _dbContext.UserSecurity.AsNoTracking().Where(userSecurity => userSecurity.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task UpdateUserSecurity(UserSecurity userSecurity)
        {
            _dbContext.Entry(userSecurity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }
    }
}
