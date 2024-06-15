using Guide.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Guide.Application.Features.Users.Queries;

public class CountUsersQueryHandler(UserManager<User> userManager) : IRequestHandler<CountUsersQuery, int>
{
    public async Task<int> Handle(CountUsersQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Search))
        {
            return await userManager.Users.Select(x => x.Id).CountAsync(cancellationToken);
        }

        var search = request.Search.ToUpper();

        return await userManager.Users
            .Where(x =>
                (x.UserName ?? "").ToUpper().Contains(search) ||
                (x.Email ?? "").ToUpper().Contains(search)
            )
            .AsNoTracking()
            .CountAsync(cancellationToken);
    }
}