using MyAccountApp.Application.ViewModels.UserSecurity;

namespace MyAccountApp.Application.Validations.UserSecurity
{
    public class UserSecurityCreateValidator : BaseValidatorUserSecurity<UserSecurityCreateViewModel>
    {
        public UserSecurityCreateValidator() {
            ValidatePassword(userSecurity => userSecurity.Password);
        }
    }
}
