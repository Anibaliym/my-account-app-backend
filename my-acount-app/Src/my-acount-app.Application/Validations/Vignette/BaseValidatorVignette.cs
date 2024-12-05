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
                .NotEmpty().WithMessage("El campo 'Id' no puede estar vacío.")
                .NotEqual(Guid.Empty).WithMessage("El campo 'Id' no es válido.");
        }

        protected void ValidateCardId(Expression<Func<T, Guid>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("El campo 'CardId' no puede estar vacío.")
                .NotEqual(Guid.Empty).WithMessage("El campo 'CardId' no es válido.");
        }

        protected void ValidateColor(Expression<Func<T, string>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("El campo 'Color' no puede estar vacío.")
                .Must(BeAValidColorType).WithMessage($"El 'Color' debe estar entre los valores permitidos ({GetAllowedColorTypes()}).")
                .MaximumLength(50).WithMessage("El campo 'Color' no debe exceder los 50 caracteres.");
        }

        protected void ValidateOrder(Expression<Func<T, int>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("El 'Order', no puede estar vacío.") // Que no sea vacío (aunque un int no puede ser nulo)
                .LessThanOrEqualTo(1000).WithMessage("El 'Order', no puede exceder el límite permitido (1000)."); // Valor máximo
        }

        private bool BeAValidColorType(string value)
        {
            // Se verifica si el valor coincide con alguno de los nombres de los enums
            return TipoColorEnum.List.Select(u => u.Name).Contains(value);
        }

        private static string GetAllowedColorTypes()
        {
            // se obtienen todos los nombres de los enums y únelos en una cadena separada por comas
            IEnumerable<string> colorTypes = TipoColorEnum.List.Select(u => u.Name);
            return string.Join(", ", colorTypes);
        }
    }
}
