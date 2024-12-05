using MyAccountApp.Application.ViewModels.Vignette;

namespace MyAccountApp.Application.Validations.Vignette
{
    public class VignetteUpdateValidator : BaseValidatorVignette<VignetteViewModel>
    {
        public VignetteUpdateValidator()
        {
            ValidateId(vignette => vignette.Id);
            ValidateCardId(vignette => vignette.CardId);
            ValidateColor(vignette => vignette.Color);
            ValidateOrder(vignette => vignette.Order);
        }
    }
}
