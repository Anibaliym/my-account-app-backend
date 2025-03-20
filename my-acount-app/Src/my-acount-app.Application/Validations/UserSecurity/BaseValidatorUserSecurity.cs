using FluentValidation;
using System.Linq.Expressions;

namespace MyAccountApp.Application.Validations.UserSecurity
{
    public abstract class BaseValidatorUserSecurity<T> : AbstractValidator<T> where T : class
    {
        protected void ValidatePassword(Expression<Func<T, string>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("El campo 'Password' no puede estar vacío.")
                .MinimumLength(5).WithMessage("El campo 'Password' debe tener al menos 5 caracteres.")
                .MaximumLength(20).WithMessage("El campo 'Password' no debe exceder los 20 caracteres.");
        }
    }
}
