namespace MyAccountApp.Application.ViewModels.Vignette
{
    public class UpdateVignetteViewModel
    {
        public Guid Id { get; set; }
        public Guid CardId { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Amount { get; set; }
        public string Color { get; set; } = string.Empty;
        public int Order { get; set; }
    }
}
