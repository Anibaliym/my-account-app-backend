using FluentValidation;
using MyAccountApp.Core.Enum.Color;
using System.Linq.Expressions;

namespace MyAccountApp.Application.Validations.Card
{
    public abstract class BaseValidatorCard<T> : AbstractValidator<T> where T : class
    {
        protected void ValidateId(Expression<Func<T, Guid>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("El campo 'Id' no puede estar vacío.")
                .NotEqual(Guid.Empty).WithMessage("El campo 'Id' no es válido.");
        }

        protected void ValidateSheetId(Expression<Func<T, Guid>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("El campo 'SheetId' no puede estar vacío.")
                .NotEqual(Guid.Empty).WithMessage("El campo 'SheetId' no es válido.");
        }

        protected void ValidateTitle(Expression<Func<T, string>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("El campo 'Title' no puede estar vacío.")
                .MaximumLength(100).WithMessage("El campo 'Title' no debe exceder los 100 caracteres.");
        }

        protected void ValidateOrder(Expression<Func<T, int>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("El 'Order', no puede estar vacío.") 
                .LessThanOrEqualTo(200).WithMessage("El 'Order', no puede exceder el límite permitido (200)."); 
        }
    }
}
