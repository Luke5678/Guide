using Guide.Domain.Common;

namespace Guide.Domain.Entities;

public class Category : AuditableEntity
{
    public ICollection<CategoryTranslation> Translations { get; set; } = new List<CategoryTranslation>();
    
    public ICollection<Attraction> Attractions { get; set; } = new List<Attraction>();
}