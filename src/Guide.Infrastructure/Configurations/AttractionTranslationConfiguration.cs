using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Guide.Domain.Entities;
using Guide.Infrastructure.Common;

namespace Guide.Infrastructure.Configurations;

public class AttractionTranslationConfiguration : AuditableEntityConfiguration<AttractionTranslation>
{
    public override void Configure(EntityTypeBuilder<AttractionTranslation> builder)
    {
        base.Configure(builder);

        builder.Property(t => t.LanguageCode).IsRequired();
        builder.Property(t => t.Name).IsRequired();
        builder.Property(t => t.Description).IsRequired();
    }
}