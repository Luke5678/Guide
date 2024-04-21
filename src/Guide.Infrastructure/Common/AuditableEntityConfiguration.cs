using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Guide.Domain.Common;

namespace Guide.Infrastructure.Common;

public abstract class AuditableEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : AuditableEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasQueryFilter(x => x.Deleted == null);
    }
}