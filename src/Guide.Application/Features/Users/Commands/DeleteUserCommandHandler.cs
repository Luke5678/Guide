using Guide.Domain.Common;
using Guide.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Guide.Application.Features.Users.Commands;

public class DeleteUserCommandHandler(UserManager<User> userManager) : IRequestHandler<DeleteUserCommand, bool>
{
    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.Id);
        if (user == null)
        {
            return false;
        }

        var isMainAdmin = await userManager.IsInRoleAsync(user, UserRoles.MainAdministrator);
        if (isMainAdmin)
        {
            return false;
        }

        var result = await userManager.DeleteAsync(user);
        return result.Succeeded;
    }
}