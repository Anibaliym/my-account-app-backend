using FluentValidation;
using System.Linq.Expressions;

namespace MyAccountApp.Application.Validations.Sheet
{
    public abstract class BaseValidatorSheet<T> : AbstractValidator<T> where T : class
    {
        protected void ValidateId(Expression<Func<T, Guid>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("The 'Id' field is required.")
                .NotEqual(Guid.Empty).WithMessage("The 'Id' field is invalid.");
        }

        protected void ValidateAccountId(Expression<Func<T, Guid>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("The 'AccountId' field is required.")
                .NotEqual(Guid.Empty).WithMessage("The 'AccountId' field is invalid.");
        }

        protected void ValidateDescription(Expression<Func<T, string>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("The 'Description' field is required.")
                .MinimumLength(3).WithMessage("The 'Description' field must contain at least 3 characters.")
                .MaximumLength(300).WithMessage("The 'Description' field must not exceed 300 characters.");
        }

        protected void ValidateOrder(Expression<Func<T, int>> expression)
        {
            RuleFor(expression)
                .GreaterThanOrEqualTo(0)
                    .WithMessage("The 'Order' field must be greater than or equal to 0.")
                .LessThanOrEqualTo(200)
                    .WithMessage("The 'Order' field must not exceed the allowed limit (200).");
        }
    }
}
