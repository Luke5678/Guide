using MediatR;

namespace Guide.Application.Features.Users.Commands;

public class DeleteUserCommand : IRequest<bool>
{
    public required string Id { get; set; }
}