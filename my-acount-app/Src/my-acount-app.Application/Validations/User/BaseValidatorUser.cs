using FluentValidation;
using MyAccountApp.Core.Enum.User;
using System.Linq.Expressions;

namespace MyAccountApp.Application.Validations.User
{
    public abstract class BaseValidatorUser<T> : AbstractValidator<T> where T : class
    {
        protected void ValidateId(Expression<Func<T, Guid>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("El campo 'Id' no puede estar vacío.")
                .NotEqual(Guid.Empty).WithMessage("El campo 'Id' no es válido.");
        }

        protected void ValidateFirstName(Expression<Func<T, string>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("El campo 'FirstName' no puede estar vacío.")
                .MinimumLength(5).WithMessage("El campo 'FirstName' debe tener al menos 5 caracteres.")
                .MaximumLength(100).WithMessage("El campo 'FirstName' no debe exceder los 100 caracteres.");
        }

        protected void ValidateLastName(Expression<Func<T, string>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("El campo 'LastName' no puede estar vacío.")
                .MinimumLength(5).WithMessage("El campo 'LastName' debe tener al menos 5 caracteres.")
                .MaximumLength(100).WithMessage("El campo 'LastName' no debe exceder los 100 caracteres.");
        }

        protected void ValidateEmail(Expression<Func<T, string>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("El campo 'Email' no puede estar vacío.")
                .EmailAddress().WithMessage("El campo 'Email' debe tener un formato correcto.");
        }

        protected void ValidateUserType(Expression<Func<T, string>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("El campo 'UserType' no puede estar vacío.")
                .Must(BeAValidUserType).WithMessage($"El 'UserType' debe estar entre los valores permitidos ({GetAllowedUserTypes()}).");
        }

        private bool BeAValidUserType(string value)
        {
            // Verifica si el valor coincide con alguno de los nombres de los enums
            return UserTypeEnum.List.Select(u => u.Name).Contains(value);
        }

        private static string GetAllowedUserTypes()
        {
            // se obtienen todos los nombres de los enums y únelos en una cadena separada por comas
            IEnumerable<string> userTypes = UserTypeEnum.List.Select(u => u.Name);
            return string.Join(", ", userTypes);
        }
    }
}
