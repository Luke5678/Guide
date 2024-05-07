namespace Guide.Domain.Entities;

public class Category
{
    public int Id { get; set; }
    public DateTime? Deleted { get; set; }
    public ICollection<CategoryTranslation> Translations { get; set; } = null!;
    
    public ICollection<Attraction> Attractions { get; set; } = null!;
}