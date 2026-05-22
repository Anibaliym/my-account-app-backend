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
                .NotEmpty().WithMessage("The 'Id' field is required.")
                .NotEqual(Guid.Empty).WithMessage("The 'Id' field is invalid.");
        }

        protected void ValidateSheetId(Expression<Func<T, Guid>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("The 'SheetId' field is required.")
                .NotEqual(Guid.Empty).WithMessage("The 'SheetId' field is invalid.");
        }

        protected void ValidateTitle(Expression<Func<T, string>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("The 'Title' field is required.")
                .MaximumLength(100).WithMessage("The 'Title' field must not exceed 100 characters.");
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
