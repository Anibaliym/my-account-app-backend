namespace MyAccountApp.Core.Entities
{
    public class Sheet
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public int CashBalance { get; set; }
        public int CurrentAccountBalance { get; set; }
    }
}
