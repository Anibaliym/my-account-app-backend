namespace MyAccountApp.Core.Entities
{
    public class Vignette
    {
        public Guid Id { get; set; }
        public Guid CardId { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Amount { get; set; }
        public string Color { get; set; } = string.Empty;
        public int Order { get; set; }
    }
}
