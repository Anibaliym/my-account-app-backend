using MyAccountApp.Application.ViewModels.User;

namespace MyAccountApp.Application.Validations.User
{
    public class UserCreateValidator : BaseValidatorUser<UserCreateViewModel>
    {
        public UserCreateValidator()
        {
            ValidateFirstName(x => x.FirstName);
            ValidateLastName(x => x.LastName);
            ValidateEmail(x => x.Email);
            ValidateUserType(x => x.UserType);
        }
    }
}
