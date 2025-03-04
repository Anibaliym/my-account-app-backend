namespace MyAccountApp.Application.ViewModels.Account
{
    public class AccountViewModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public int Order { get; set; }
    }
}
