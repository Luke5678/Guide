namespace Guide.Domain.Entities;

public class AttractionImage
{
    public int Id { get; set; }
    public string Path { get; set; } = null!;
    public bool IsMain { get; set; }

    public int? AttractionId { get; set; }
    public Attraction? Attraction { get; set; }

    public string Url => $"/uploads/{Path}";
}