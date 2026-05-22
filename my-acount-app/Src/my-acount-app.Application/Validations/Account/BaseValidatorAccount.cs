using FluentValidation;
using System.Linq.Expressions;

namespace MyAccountApp.Application.Validations.Account
{
    public abstract class BaseValidatorAccount<T> : AbstractValidator<T> where T : class
    {
        protected void ValidateId(Expression<Func<T, Guid>> idExpression)
        {
            RuleFor(idExpression)
                .NotEmpty().WithMessage("The 'Id' field is required.")
                .NotEqual(Guid.Empty).WithMessage("The 'Id' field is invalid.");
        }

        protected void ValidateDescription(Expression<Func<T, string>> descriptionExpression)
        {
            RuleFor(descriptionExpression)
                .NotEmpty().WithMessage("The 'Description' field is required.")
                .MinimumLength(3).WithMessage("The 'Description' field must contain at least 3 characters.")
                .MaximumLength(300).WithMessage("The 'Description' field must not exceed 300 characters.");
        }

        protected void ValidateCreationDate(Expression<Func<T, string>> creationDateExpression)
        {
            RuleFor(creationDateExpression)
                .NotEmpty().WithMessage("The 'CreationDate' field is required.")
                .MinimumLength(10).WithMessage("The 'CreationDate' field must contain at least 10 characters.")
                .MaximumLength(10).WithMessage("The 'CreationDate' field must not exceed 10 characters.");
        }
    }
}
