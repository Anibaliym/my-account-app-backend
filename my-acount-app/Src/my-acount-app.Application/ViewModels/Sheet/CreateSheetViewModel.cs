namespace MyAccountApp.Application.ViewModels.Sheet
{
    public class CreateSheetViewModel
    {
        public Guid AccountId { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
