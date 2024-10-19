using MyAccountApp.Core.Entities;

namespace MyAccountApp.Core.Interfaces
{
    public interface ICardRepository
    {
        Task<Card> GetCardById(Guid id);
        Task<IEnumerable<Card>> GetCardBySheetId(Guid sheetId);
        Task CreateCard(Card model);
        Task UpdateCard(Card model);
        Task<bool> DeleteCard(Guid id);
    }
}
