namespace MyAccountApp.Application.ViewModels.UserSecurity
{
    public class UserSecurityUpdateViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        public DateTime LastPasswordChangeDate { get; set; }
    }
}
