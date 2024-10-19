using MyAccountApp.Application.ViewModels.Account;

namespace MyAccountApp.Application.Validations.Account
{
    public class AccountCreateValidator : BaseValidatorAccount<CreateAccountViewModel>
    {
        public AccountCreateValidator()
        {
            ValidateId(account => account.UserId);
            ValidateDescription(account => account.Description);
        }
    }
}
