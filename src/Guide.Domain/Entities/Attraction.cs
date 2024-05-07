using Guide.Domain.Common;

namespace Guide.Domain.Entities;

public class Attraction : AuditableEntity
{
    public ICollection<Category> Categories { get; set; } = null!;
    public ICollection<AttractionTranslation> Translations { get; set; } = null!;
}