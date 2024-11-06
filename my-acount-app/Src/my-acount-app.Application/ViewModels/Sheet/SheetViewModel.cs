namespace MyAccountApp.Application.ViewModels.Sheet
{
    public class SheetViewModel
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public int cashBalance { get; set; }
        public int currentAccountBalance { get; set; }
        public int Order { get; set; }
    }
}
