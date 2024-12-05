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

        protected void ValidateColor(Expression<Func<T, string>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("El campo 'Color' no puede estar vacío.")
                .Must(BeAValidColorType).WithMessage($"El 'Color' debe estar entre los valores permitidos ({GetAllowedColorTypes()}).")
                .MaximumLength(50).WithMessage("El campo 'Color' no debe exceder los 50 caracteres.");
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
