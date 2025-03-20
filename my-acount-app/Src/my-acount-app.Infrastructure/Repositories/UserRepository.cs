using Microsoft.EntityFrameworkCore;
using MyAccountApp.Core.Entities;
using MyAccountApp.Core.Interfaces;
using MyAccountApp.Infrastructure.Context;
using Npgsql;

namespace MyAccountApp.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly my_account_appAppDbContext _dbContext;

        public UserRepository(my_account_appAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetUserById(Guid id)
        {
            return await _dbContext.User.AsNoTracking().Where(user => user.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByEmail(string email)
        {
            return await _dbContext.User.AsNoTracking().Where(user => user.Email == email).FirstOrDefaultAsync();
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _dbContext.User.ToListAsync();
        }

        public async Task CreateUser(User user)
        {
            await _dbContext.User.AddAsync(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateUser(User user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteUser(Guid id)
        {
            try
            {
                User user = await _dbContext.User.FindAsync(id);

                if (user != null)
                {
                    _dbContext.User.Remove(user);
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
