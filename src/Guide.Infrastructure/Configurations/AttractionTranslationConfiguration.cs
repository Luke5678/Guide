using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Guide.Domain.Entities;
using Guide.Infrastructure.Common;

namespace Guide.Infrastructure.Configurations;

public class AttractionTranslationConfiguration : IEntityTypeConfiguration<AttractionTranslation>
{
    public void Configure(EntityTypeBuilder<AttractionTranslation> builder)
    {
        builder.Property(t => t.LanguageCode).HasMaxLength(5).IsRequired();
        builder.Property(t => t.Name).HasMaxLength(255).IsRequired();
        builder.Property(t => t.Description).HasMaxLength(10240).IsRequired();
    }
}