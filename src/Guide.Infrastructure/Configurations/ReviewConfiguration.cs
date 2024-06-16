using Guide.Domain.Entities;
using Guide.Infrastructure.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Guide.Infrastructure.Configurations;

public class ReviewConfiguration : AuditableEntityConfiguration<Review>
{
    public override void Configure(EntityTypeBuilder<Review> builder)
    {
        base.Configure(builder);
        
        builder.HasOne(x => x.Attraction).WithMany(x => x.Reviews);
        builder.HasOne(x => x.User).WithMany(x => x.Reviews);
    }
}