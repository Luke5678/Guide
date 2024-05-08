using Guide.Domain.Entities;
using Guide.Infrastructure.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Guide.Infrastructure.Configurations;

public class CategoryConfiguration : AuditableEntityConfiguration<Category>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);

        builder.HasMany(x => x.Translations).WithOne(x => x.Category);
    }
}