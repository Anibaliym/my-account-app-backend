using Microsoft.EntityFrameworkCore;
using MyAccountApp.Core.Entities;
using MyAccountApp.Core.Interfaces;
using MyAccountApp.Infrastructure.Context;
using Npgsql;

namespace MyAccountApp.Infrastructure.Repositories
{
    public class VignetteRepository : IVignetteRepository
    {
        private readonly my_account_appAppDbContext _dbContext;

        public VignetteRepository(my_account_appAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Vignette?> GetVignetteById(Guid id)
        {
            return await _dbContext.Vignette.FindAsync(id);
        }

        public async Task<IEnumerable<Vignette>> GetVignetteByCardId(Guid cardId)
        {
            return await _dbContext.Vignette.AsNoTracking().Where(vignette => vignette.CardId == cardId).ToListAsync(); 
        }
        public async Task<int> GetNextOrderByCardId(Guid cardId)
        {
            return await _dbContext.Vignette
                .AsNoTracking()
                .Where(vignette => vignette.CardId == cardId)
                .MaxAsync(vignette => (int?)vignette.Order) + 1 ?? 1;
        }

        public async Task<int> GetTotalVignettesCard(Guid cardId)
        {
            return await _dbContext.Vignette
                .AsNoTracking()
                .Where(vignette => vignette.CardId == cardId)
                .CountAsync();
        }

        public async Task CreateVignette(Vignette model)
        {
            await _dbContext.Vignette.AddAsync(model);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateVignette(Vignette model)
        {
            _dbContext.Entry(model).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteVignette(Guid id)
        {
            try
            {

                Vignette? vignette = await _dbContext.Vignette.FindAsync(id);

                if (vignette != null)
                {
                    _dbContext.Vignette.Remove(vignette);
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
