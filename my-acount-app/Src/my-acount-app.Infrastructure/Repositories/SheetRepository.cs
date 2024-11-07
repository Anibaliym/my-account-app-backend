using Microsoft.EntityFrameworkCore;
using MyAccountApp.Core.Entities;
using MyAccountApp.Core.Interfaces;
using MyAccountApp.Infrastructure.Context;
using Npgsql;

namespace MyAccountApp.Infrastructure.Repositories
{
    public class SheetRepository : ISheetRepository
    {
        private readonly my_account_appAppDbContext _dbContext;

        public SheetRepository(my_account_appAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Sheet?> GetSheetById(Guid id)
        {
            return await _dbContext.Sheet.FindAsync(id);
        }

        public async Task<Sheet?> GetSheetAccountByOrder(int order, Guid accountId)
        {
            return await _dbContext.Sheet.Where(sheet => sheet.Order == order && sheet.AccountId == accountId).FirstOrDefaultAsync();
        }
        
        public async Task<int> GetNextOrderByAccountId(Guid accountId)
        {
            return await _dbContext.Sheet.Where(hoja => hoja.AccountId == accountId).MaxAsync(hoja => (int?)hoja.Order) + 1 ?? 1;
        }

        public async Task<IEnumerable<Sheet>> GetSheetByAccountId(Guid accountId)
        {

            var x = await _dbContext.Sheet.AsNoTracking().Where(hoja => hoja.AccountId == accountId).ToListAsync(); 
            return await _dbContext.Sheet.AsNoTracking().Where(hoja => hoja.AccountId == accountId).ToListAsync();
        }

        public async Task CreateSheet(Sheet model)
        {
            await _dbContext.Sheet.AddAsync(model);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateSheet(Sheet model)
        {
            _dbContext.Entry(model).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteSheet(Guid id)
        {
            try
            {
                Sheet? sheet = await _dbContext.Sheet.FindAsync(id);

                if (sheet != null)
                {
                    _dbContext.Sheet.Remove(sheet);
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
