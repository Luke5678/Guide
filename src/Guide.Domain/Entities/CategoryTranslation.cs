namespace Guide.Domain.Entities;

public class CategoryTranslation
{
    public int Id { get; set; }
    public string LanguageCode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public Category Category { get; set; } = null!;
}