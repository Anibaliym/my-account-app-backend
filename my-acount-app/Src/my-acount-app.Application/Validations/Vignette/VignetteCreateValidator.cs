﻿using MyAccountApp.Application.ViewModels.Vignette;

namespace MyAccountApp.Application.Validations.Vignette
{
    public class VignetteCreateValidator : BaseValidatorVignette<VignetteCreateViewModel>
    {
        public VignetteCreateValidator()
        {
            ValidateCardId(vignette => vignette.CardId);
        }
    }
}
