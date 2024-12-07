namespace MyAccountApp.Application.ViewModels.Vignette
{
    public class VignetteCreateViewModel
    {
        public Guid CardId { get; set; }
        public string Description { get; set; } = string.Empty;
        public int Amount { get; set; }
    }
}
