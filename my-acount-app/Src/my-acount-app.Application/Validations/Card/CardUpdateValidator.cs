using MyAccountApp.Application.ViewModels.Card;

namespace MyAccountApp.Application.Validations.Card
{
    public class CardUpdateValidator : BaseValidatorCard<UpdateCardViewModel>
    {
        public CardUpdateValidator()
        {
            ValidateId(card => card.Id); 
            ValidateSheetId(card => card.SheetId); 
            ValidateTitle(card => card.Title);
            ValidateOrder(card => card.Order);
        }
    }
}
