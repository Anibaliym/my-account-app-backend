using MyAccountApp.Application.ViewModels.Card;

namespace MyAccountApp.Application.Validations.Card
{
    public class CardCreateValidator : BaseValidatorCard<CreateCardViewModel>
    {
        public CardCreateValidator()
        {
            ValidateSheetId(card => card.SheetId);
            ValidateTitle(card => card.Title);
        }
    }
}
