using Guide.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Guide.Application.Features.Users.Queries;

public class GetUsersQueryHandler(UserManager<User> userManager) : IRequestHandler<GetUsersQuery, List<User>>
{
    public async Task<List<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var query = userManager.Users.AsNoTracking();

        if (!string.IsNullOrEmpty(request.Search))
        {
            var search = request.Search.ToUpper();
            query = query.Where(x =>
                (x.UserName ?? "").ToUpper().Contains(search) ||
                (x.Email ?? "").ToUpper().Contains(search)
            );
        }

        if (string.IsNullOrEmpty(request.OrderBy))
        {
            query = query.OrderBy(x => x.Id);
        }
        else
        {
            query = request.OrderBy.ToLower() switch
            {
                "id asc" => query.OrderBy(x => x.Id),
                "id desc" => query.OrderByDescending(x => x.Id),
                "name asc" => query.OrderBy(x => x.UserName),
                "name desc" => query.OrderByDescending(x => x.UserName),
                "email asc" => query.OrderBy(x => x.Email),
                "email desc" => query.OrderByDescending(x => x.Email),
                _ => query
            };
        }

        if (request.Page > 0 && request.Limit > 0)
        {
            query = query.Skip((request.Page - 1) * request.Limit).Take(request.Limit);
        }

        return await query.ToListAsync(cancellationToken);
    }
}