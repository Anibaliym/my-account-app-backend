using FluentValidation;

namespace MyAccountApp.Application.Validations.GenericValidation
{
    public class IdValidator : AbstractValidator<Guid>
    {
        public IdValidator()
        {
            RuleFor(id => id)
                .NotEmpty().WithMessage("El campo 'Id' no puede estar vacío.")
                .NotEqual(Guid.Empty).WithMessage("El campo 'Id' no es válido.");
        }
    }
}
