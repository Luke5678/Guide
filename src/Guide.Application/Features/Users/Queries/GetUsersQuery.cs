using Guide.Domain.Entities;
using MediatR;

namespace Guide.Application.Features.Users.Queries;

public class GetUsersQuery : IRequest<List<User>>
{
    public int Page { get; set; }
    public int Limit { get; set; }
    public string? OrderBy { get; set; }
    public string? Search { get; set; }
}