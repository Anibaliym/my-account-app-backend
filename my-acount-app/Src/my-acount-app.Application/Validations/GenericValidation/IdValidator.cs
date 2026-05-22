using FluentValidation;

namespace MyAccountApp.Application.Validations.GenericValidation
{
    public class IdValidator : AbstractValidator<Guid>
    {
        public IdValidator()
        {
            RuleFor(id => id)
                .NotEmpty().WithMessage("The 'Id' field is required.")
                .NotEqual(Guid.Empty).WithMessage("The 'Id' field is invalid.");        
        }
    }
}
