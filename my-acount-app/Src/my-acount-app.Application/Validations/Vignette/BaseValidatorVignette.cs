using FluentValidation;
using MyAccountApp.Core.Enum.Color;
using System.Linq.Expressions;

namespace MyAccountApp.Application.Validations.Vignette
{
    public abstract class BaseValidatorVignette<T> : AbstractValidator<T> where T : class
    {
        protected void ValidateId(Expression<Func<T, Guid>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("The 'Id' field is required.")
                .NotEqual(Guid.Empty).WithMessage("The 'Id' field is invalid.");
        }

        protected void ValidateCardId(Expression<Func<T, Guid>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("The 'CardId' field is required.")
                .NotEqual(Guid.Empty).WithMessage("The 'CardId' field is invalid.");
        }

        protected void ValidateColor(Expression<Func<T, string>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("The 'Color' field is required.")
                .Must(BeAValidColorType)
                    .WithMessage($"The 'Color' field must contain one of the allowed values ({GetAllowedColorTypes()}).")
                .MaximumLength(50)
                    .WithMessage("The 'Color' field must not exceed 50 characters.");
        }

        protected void ValidateOrder(Expression<Func<T, int>> expression)
        {
            RuleFor(expression)
                .GreaterThanOrEqualTo(0)
                    .WithMessage("The 'Order' field must be greater than or equal to 0.")
                .LessThanOrEqualTo(1000)
                    .WithMessage("The 'Order' field must not exceed the allowed limit (1000).");
        }

        private bool BeAValidColorType(string value)
        {
            return TipoColorEnum.List
                .Select(u => u.Name)
                .Contains(value);
        }

        private static string GetAllowedColorTypes()
        {
            IEnumerable<string> colorTypes = TipoColorEnum.List.Select(u => u.Name);
            return string.Join(", ", colorTypes);
        }
    }
}