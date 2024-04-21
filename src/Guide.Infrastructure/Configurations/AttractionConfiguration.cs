using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Guide.Domain.Entities;
using Guide.Infrastructure.Common;

namespace Guide.Infrastructure.Configurations;

public class AttractionConfiguration : AuditableEntityConfiguration<Attraction>
{
    public override void Configure(EntityTypeBuilder<Attraction> builder)
    {
        base.Configure(builder);

        builder.Property(t => t.Name).IsRequired();
        builder.Property(t => t.Description).IsRequired();
        builder.Property(t => t.Category).IsRequired();
    }
}