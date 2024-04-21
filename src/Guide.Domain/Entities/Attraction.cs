using Guide.Domain.Common;

namespace Guide.Domain.Entities;

public class Attraction : AuditableEntity
{
    public string Name { get; set; }
    public string Category { get; set; }
    public string Description { get; set; }
}