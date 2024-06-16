using Guide.Domain.Common;

namespace Guide.Domain.Entities;

public class Review : AuditableEntity
{
    public string Comment { get; set; } = "";
    public byte Rating { get; set; }

    public int AttractionId { get; set; }
    public Attraction Attraction { get; set; } = null!;

    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;
}