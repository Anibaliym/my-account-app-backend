using Microsoft.EntityFrameworkCore;
using MyAccountApp.Core.Entities;
using MyAccountApp.Core.Interfaces;
using MyAccountApp.Infrastructure.Context;
using Npgsql;

namespace MyAccountApp.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly my_account_appAppDbContext _dbContext;

        public AccountRepository(my_account_appAppDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task<Account?> GetAccountById(Guid id) {
            return await _dbContext.Account
                .AsNoTracking()
                .Where(account => account.Id == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Account>> GetAccountByUserId(Guid userId) {
            return await _dbContext.Account
            .AsNoTracking()
            .Where(account => account.UserId == userId)
            .OrderBy(account => account.Order)
            .ToListAsync();
        }
        
        public async Task<int> GetTotalUserAccounts(Guid userId)
        {
            return await _dbContext.Account
                .AsNoTracking()
                .Where(account => account.UserId == userId)
                .CountAsync();
        }

        public async Task CreateAccount(Account modelo) {
            await _dbContext.Account.AddAsync(modelo);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> GetNextAccountOrderByUserId(Guid userId)
        {
            return await _dbContext.Account
                .AsNoTracking()
                .Where(account => account.UserId == userId)
                .MaxAsync(account => (int?)account.Order) + 1 ?? 1;
        }

        public async Task UpdateAccount(Account model) {
            var existingAccount = await _dbContext.Account.FindAsync(model.Id);
            if (existingAccount != null) {
                _dbContext.Entry(existingAccount).CurrentValues.SetValues(model);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> DeleteAccount(Guid id)
        {
            try 
            {
                Account? account = await _dbContext.Account.FindAsync(id);

                if (account != null)
                {
                    _dbContext.Account.Remove(account);
                    await _dbContext.SaveChangesAsync();
                }

                return true;
            }
            catch (DbUpdateException dbEx)
            {
                // Verifica si el error está relacionado con restricciones de integridad referencial (Foreign Key)
                if (dbEx.InnerException is PostgresException pgEx && pgEx.SqlState == "23503") // Código de error para violación de clave foránea
                {
                    Console.WriteLine("No se puede eliminar el registro porque está siendo referenciado por otra tabla.");
                    return false;
                }

                // Si la excepción no está relacionada con claves foráneas, la volvemos a lanzar
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
