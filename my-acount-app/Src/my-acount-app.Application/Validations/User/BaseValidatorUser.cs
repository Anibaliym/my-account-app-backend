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
                .NotEmpty().WithMessage("The 'Id' field is required.")
                .NotEqual(Guid.Empty).WithMessage("The 'Id' field is invalid.");
        }

        protected void ValidateFirstName(Expression<Func<T, string>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("The 'FirstName' field is required.")
                .MinimumLength(3).WithMessage("The 'FirstName' field must contain at least 3 characters.")
                .MaximumLength(100).WithMessage("The 'FirstName' field must not exceed 100 characters.");
        }

        protected void ValidateLastName(Expression<Func<T, string>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("The 'LastName' field is required.")
                .MinimumLength(3).WithMessage("The 'LastName' field must contain at least 3 characters.")
                .MaximumLength(100).WithMessage("The 'LastName' field must not exceed 100 characters.");
        }

        protected void ValidateEmail(Expression<Func<T, string>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("The 'Email' field is required.")
                .EmailAddress().WithMessage("The 'Email' field must contain a valid email address.");
        }

        protected void ValidateUserType(Expression<Func<T, string>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("The 'UserType' field is required.")
                .Must(BeAValidUserType)
                .WithMessage($"The 'UserType' field must contain one of the allowed values ({GetAllowedUserTypes()}).");
        }

        protected void ValidateUserRegistrationMethod(Expression<Func<T, string>> expression)
        {
            RuleFor(expression)
                .NotEmpty().WithMessage("The 'RegistrationMethod' field is required.")
                .Must(BeAValidUserRegistrationMethodType)
                .WithMessage($"The 'RegistrationMethod' field must contain one of the allowed values ({GetAllUserRegistrationMethodTypes()}).");
        }

        private bool BeAValidUserRegistrationMethodType(string value)
        {
            return UserRegistrationMethodEnum.List
                .Select(u => u.Name)
                .Contains(value);
        }

        private static string GetAllUserRegistrationMethodTypes()
        {
            IEnumerable<string> userTypes = UserRegistrationMethodEnum.List.Select(u => u.Name);
            return string.Join(", ", userTypes);
        }

        private bool BeAValidUserType(string value)
        {
            return UserTypeEnum.List
                .Select(u => u.Name)
                .Contains(value);
        }

        private static string GetAllowedUserTypes()
        {
            IEnumerable<string> userTypes = UserTypeEnum.List.Select(u => u.Name);
            return string.Join(", ", userTypes);
        }
    }
}