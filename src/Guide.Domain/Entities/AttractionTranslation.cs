namespace Guide.Domain.Entities;

public class AttractionTranslation
{
    public int Id { get; set; }
    public string LanguageCode { get; set; } = null!;
    
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    
    public Attraction Attraction { get; set; } = null!;
}