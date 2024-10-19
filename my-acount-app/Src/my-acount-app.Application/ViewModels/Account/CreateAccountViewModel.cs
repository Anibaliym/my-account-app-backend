namespace MyAccountApp.Application.ViewModels.Account
{
    public class CreateAccountViewModel
    {
        public Guid UserId { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
