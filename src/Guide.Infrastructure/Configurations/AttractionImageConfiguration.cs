using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Guide.Domain.Entities;

namespace Guide.Infrastructure.Configurations;

public class AttractionImageConfiguration : IEntityTypeConfiguration<AttractionImage>
{
    public void Configure(EntityTypeBuilder<AttractionImage> builder)
    {
        builder.HasOne(x => x.Attraction).WithMany(x => x.Images);
    }
}