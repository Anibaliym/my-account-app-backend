namespace MyAccountApp.Application.Services 
{
public class SheetCardsWithVignetteViewModel
{
    public List<CardWithVignettesDTO> Cards { get; set; } = new List<CardWithVignettesDTO>();
}

public class CardWithVignettesDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreationDate { get; set; }
    public List<VignetteDTO> Vignettes { get; set; } = new List<VignetteDTO>();
    public int TotalCardAmount { get; set; }
    public int Order { get; set; }
}

public class VignetteDTO
{
    public Guid Id { get; set; }
    public string Description { get; set; } = string.Empty;
    public int Amount { get; set; }
    public string Color { get; set; } = string.Empty;
    public int Order { get; set; }
}
}

