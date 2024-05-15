using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Guide.Domain.Entities;
using Guide.Infrastructure.Common;

namespace Guide.Infrastructure.Configurations;

public class AttractionConfiguration : AuditableEntityConfiguration<Attraction>
{
    public override void Configure(EntityTypeBuilder<Attraction> builder)
    {
        base.Configure(builder);

        builder.HasMany(c => c.Categories).WithMany(x => x.Attractions);
        builder.HasMany(c => c.Images).WithOne(x => x.Attraction);
        builder.HasMany(c => c.Translations).WithOne(t => t.Attraction);
    }
}