using Guide.Domain.Common;

namespace Guide.Domain.Entities;

public class AttractionTranslation : AuditableEntity
{
    public int Id { get; set; }
    public string LanguageCode { get; set; }
    
    public string Name { get; set; }
    public string Description { get; set; }
    
    public Attraction Attraction { get; set; }
}