using System.Reflection;
using System.Security.Claims;
using Guide.Domain.Common;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Guide.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Guide.Infrastructure;

public class GuideDbContext(
    DbContextOptions<GuideDbContext> options,
    IHttpContextAccessor httpContext)
    : IdentityDbContext<User>(options)
{
    public DbSet<Attraction> Attractions { get; set; }
    public DbSet<AttractionImage> AttractionImages { get; set; }
    public DbSet<AttractionTranslation> AttractionTranslations { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryTranslation> CategoryTranslations { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        var currentUser = httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.Email) ?? "no data";

        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Created = DateTime.Now;
                    entry.Entity.CreatedBy = currentUser;
                    entry.Entity.Modified = DateTime.Now;
                    entry.Entity.ModifiedBy = currentUser;
                    break;
                case EntityState.Modified:
                    entry.Entity.Modified = DateTime.Now;
                    entry.Entity.ModifiedBy = currentUser;
                    break;
                case EntityState.Deleted:
                    entry.Entity.Modified = DateTime.Now;
                    entry.Entity.ModifiedBy = currentUser;
                    entry.Entity.Deleted = DateTime.Now;
                    entry.Entity.DeletedBy = currentUser;
                    entry.State = EntityState.Modified;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}