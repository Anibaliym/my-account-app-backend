namespace MyAccountApp.Application.ViewModels.Sheet
{
    public class SheetViewModel
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Descripcion { get; set; } = string.Empty;
        public DateTime CreationDate { get; set; }
        public int SaldoEfectivo { get; set; }
        public int SaldoCtaCte { get; set; }
    }
}
