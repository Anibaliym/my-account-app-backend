using MyAccountApp.Application.Responses;
using MyAccountApp.Application.ViewModels.Card;

namespace MyAccountApp.Application.Interfaces
{
    public interface ICardAppService : IDisposable
    {
        Task<CardViewModel> GetCardById(Guid id);
        Task<IEnumerable<CardViewModel>> GetCardBySheetId(Guid sheetId); 
        Task<GenericResponse> CreateCard(CreateCardViewModel model);
        Task<GenericResponse> UpdateCard(UpdateCardViewModel model);
        Task<GenericResponse> DeleteCard(Guid id);
    }
}
