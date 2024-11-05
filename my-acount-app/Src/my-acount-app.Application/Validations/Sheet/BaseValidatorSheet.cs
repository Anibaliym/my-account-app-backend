using FluentValidation;
using System.Linq.Expressions;

namespace MyAccountApp.Application.Validations.Sheet
{
    public abstract class BaseValidatorSheet<T> : AbstractValidator<T> where T : class
    {
        protected void ValidateId(Expression<Func<T, Guid>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("El campo 'Id' no puede estar vacío.")
                .NotEqual(Guid.Empty).WithMessage("El campo 'Id' no es válido.");
        }

        protected void ValidateAccountId(Expression<Func<T, Guid>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("El campo 'AccountId' no puede estar vacío.")
                .NotEqual(Guid.Empty).WithMessage("El campo 'AccountId' no es válido.");
        }

        protected void ValidateDescription(Expression<Func<T, string>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("El campo 'Description' no puede estar vacío.")
                .MinimumLength(5).WithMessage("El campo 'Description' debe tener al menos 5 caracteres.")
                .MaximumLength(300).WithMessage("El campo 'Description' no debe exceder los 300 caracteres.");
        }

        protected void ValidateOrder(Expression<Func<T, int>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("El 'Order', no puede estar vacío.") 
                .LessThanOrEqualTo(200).WithMessage("El 'Order', no puede exceder el límite permitido (200)."); 
        }
    }
}
