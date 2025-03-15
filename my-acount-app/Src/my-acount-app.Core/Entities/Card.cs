namespace MyAccountApp.Core.Entities
{
    public class Card
    {
        public Guid Id { get; set; }
        public Guid SheetId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public int Order { get; set; }
    }
}
