using MyAccountApp.Application.ViewModels.User;

namespace MyAccountApp.Application.Validations.User
{
    public class UserUpdateValidator : BaseValidatorUser<UserUpdateViewModel>
    {
        public UserUpdateValidator()
        {
            ValidateId(x => x.Id);
            ValidateFirstName(x => x.FirstName);
            ValidateLastName(x => x.LastName);
            ValidateEmail(x => x.Email);
            ValidateUserType(x => x.UserType);
        }
    }
}
