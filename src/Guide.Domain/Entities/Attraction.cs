using Guide.Domain.Common;

namespace Guide.Domain.Entities;

public class Attraction : AuditableEntity
{
    public int Id { get; set; }
    public ICollection<Category> Categories { get; set; }
    public ICollection<AttractionTranslation> Translations { get; set; }
}