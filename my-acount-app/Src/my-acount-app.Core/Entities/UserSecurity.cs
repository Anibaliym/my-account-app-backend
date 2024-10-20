namespace MyAccountApp.Core.Entities
{
    public class UserSecurity
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        public DateTime LastPasswordChangeDate { get; set; }
    }
}
