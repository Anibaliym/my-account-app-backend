using MyAccountApp.Application.ViewModels.UserSecurity;

namespace MyAccountApp.Application.ViewModels.User
{
    public class UserCreateViewModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RegistrationMethod { get; set; } = string.Empty;
        public UserSecurityCreateViewModel? UserSecurity { get; set; }
    }
}
