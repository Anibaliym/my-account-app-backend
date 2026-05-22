using FluentValidation;
using System.Linq.Expressions;

namespace MyAccountApp.Application.Validations.UserSecurity
{
    public abstract class BaseValidatorUserSecurity<T> : AbstractValidator<T> where T : class
    {
        protected void ValidatePassword(Expression<Func<T, string>> expression)
        {
            RuleFor(expression)
                .NotEmpty()
                    .WithMessage("The 'Password' field is required.")
                .MinimumLength(5)
                    .WithMessage("The 'Password' field must contain at least 5 characters.")
                .MaximumLength(20)
                    .WithMessage("The 'Password' field must not exceed 20 characters.");        }
    }
}
