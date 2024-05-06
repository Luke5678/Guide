namespace Guide.Domain.Entities;

public class CategoryTranslation
{
    public int Id { get; set; }
    public string LanguageCode { get; set; }
    
    public string Name { get; set; }

    public Category Category { get; set; }
}