using Guide.Domain.Common;
using Guide.Domain.Entities;
using Guide.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Guide.Application.Features.Reviews.Commands;

public class DeleteReviewCommandHandler(
    GuideDbContext dbContext,
    UserManager<User> userManager,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<DeleteReviewCommand>
{
    public async Task Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        var claimsPrincipal = httpContextAccessor.HttpContext?.User;
        if (claimsPrincipal == null) return;

        var user = await userManager.GetUserAsync(claimsPrincipal);
        if (user == null) return;

        var review = await dbContext.Reviews
            .Where(x => x.Id == request.ReviewId).Select(x => new { x.Id, x.UserId })
            .FirstOrDefaultAsync(cancellationToken);
        if (review == null) return;

        var isAllowed = user.Id == review.UserId;

        if (!isAllowed)
        {
            isAllowed = await userManager.IsInRoleAsync(user, UserRoles.Administrator);
        }

        if (isAllowed)
        {
            await dbContext.Reviews
                .Where(x => x.Id == review.Id)
                .ExecuteDeleteAsync(cancellationToken);
        }
    }
}