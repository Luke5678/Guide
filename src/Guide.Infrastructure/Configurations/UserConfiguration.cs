using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Guide.Domain.Entities;

namespace Guide.Infrastructure.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.Property(p => p.Id).ValueGeneratedOnAdd();
        builder.HasMany(c => c.Reviews).WithOne(x => x.User);
        
        builder.HasQueryFilter(x => !x.IsDeleted);
    }
}