namespace MyAccountApp.Application.ViewModels.Card
{
    public class CardViewModel
    {
        public Guid Id { get; set; }
        public Guid SheetId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public string Color { get; set; } = string.Empty;
    }
}
