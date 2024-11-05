namespace MyAccountApp.Application.ViewModels.Sheet
{
    public class UpdateSheetViewModel
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Description { get; set; } = string.Empty;
        public int CashBalance { get; set; }
        public int CurrentAccountBalance { get; set; }
        public int Order { get; set; }
    }
}
