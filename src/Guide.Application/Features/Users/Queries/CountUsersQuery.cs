using MediatR;

namespace Guide.Application.Features.Users.Queries;

public class CountUsersQuery : IRequest<int>
{
    public string? Search { get; set; }
}