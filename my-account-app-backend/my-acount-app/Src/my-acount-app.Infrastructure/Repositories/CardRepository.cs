using Microsoft.EntityFrameworkCore;
using MyAccountApp.Core.Entities;
using MyAccountApp.Core.Interfaces;
using MyAccountApp.Infrastructure.Context;
using Npgsql;


namespace MyAccountApp.Infrastructure.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly my_account_appAppDbContext _dbContext;

        public CardRepository(my_account_appAppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Card?> GetCardById(Guid id)
        {
            return await _dbContext.Card.FindAsync(id);
        }

        public async Task<IEnumerable<Card>> GetCardBySheetId(Guid sheetId)
        {
            return await _dbContext.Card.AsNoTracking().Where(card => card.SheetId == sheetId).ToListAsync();
        }

        public async Task CreateCard(Card model)
        {
            await _dbContext.Card.AddAsync(model);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateCard(Card model)
        {
            _dbContext.Entry(model).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> DeleteCard(Guid id)
        {
            try
            {
                Card? card = await _dbContext.Card.FindAsync(id);
                if (card != null)
                {
                    _dbContext.Card.Remove(card);
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
