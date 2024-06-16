using Microsoft.AspNetCore.Identity;

namespace Guide.Domain.Entities;

public class User : IdentityUser
{
    public bool IsDeleted { get; set; }
    public ICollection<Review> Reviews { get; set; } = null!;
}