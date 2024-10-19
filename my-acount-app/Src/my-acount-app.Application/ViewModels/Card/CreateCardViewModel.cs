namespace MyAccountApp.Application.ViewModels.Card
{
    public class CreateCardViewModel
    {
        public Guid SheetId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }
}
