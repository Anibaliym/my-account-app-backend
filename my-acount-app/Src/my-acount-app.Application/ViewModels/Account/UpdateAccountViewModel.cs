namespace MyAccountApp.Application.ViewModels.Account
{
    public class UpdateAccountViewModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Order { get; set; }
    }
}
