using MyAccountApp.Application.ViewModels.Vignette;

namespace MyAccountApp.Application.Validations.Vignette
{
    public class VignetteUpdateValidator : BaseValidatorVignette<VignetteViewModel>
    {
        public VignetteUpdateValidator()
        {
            ValidateId(vignette => vignette.Id);
            ValidateCardId(vignette => vignette.CardId);
            ValidateDescription(vignette => vignette.Description);
            ValidateAmount(vignette => vignette.Amount);
            ValidateColor(vignette => vignette.Color);
            ValidateOrder(vignette => vignette.Order);
        }
    }
}
