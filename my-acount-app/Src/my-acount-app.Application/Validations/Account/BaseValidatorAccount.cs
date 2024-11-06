using FluentValidation;
using System.Linq.Expressions;

namespace MyAccountApp.Application.Validations.Account
{
    public abstract class BaseValidatorAccount<T> : AbstractValidator<T> where T : class
    {
        protected void ValidateId(Expression<Func<T, Guid>> idExpression)
        {
            RuleFor(idExpression)
                .NotEmpty().WithMessage("El campo 'Id' no puede estar vacío.")
                .NotEqual(Guid.Empty).WithMessage("El campo 'Id' no es válido.");
        }

        protected void ValidateUserId(Expression<Func<T, Guid>> idExpression)
        {
            RuleFor(idExpression)
                .NotEmpty().WithMessage("El campo 'UserId' no puede estar vacío.")
                .NotEqual(Guid.Empty).WithMessage("El campo 'UserId' no es válido.");
        }

        protected void ValidateDescription(Expression<Func<T, string>> descripcionExpression)
        {
            RuleFor(descripcionExpression)
                .NotEmpty().WithMessage("El campo 'Description' no puede estar vacío.")
                .MinimumLength(3).WithMessage("El campo 'Descritcion' debe tener al menos 3 caracteres.")
                .MaximumLength(300).WithMessage("El campo 'Descritcion' no debe exceder los 300 caracteres.");
        }

        protected void ValidateCreationDate(Expression<Func<T, string>> fechaCreacionExpression)
        {
            RuleFor(fechaCreacionExpression)
                .NotEmpty().WithMessage("El campo 'CreationDate' no puede estar vacío.")
                .MinimumLength(10).WithMessage("El campo 'CreationDate' debe tener al menos 10 caracteres.")
                .MaximumLength(10).WithMessage("El campo 'CreationDate' no debe exceder los 10 caracteres.");
        }
    }
}
