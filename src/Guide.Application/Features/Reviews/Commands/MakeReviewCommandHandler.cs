using Guide.Domain.Entities;
using Guide.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Guide.Application.Features.Reviews.Commands;

public class MakeReviewCommandHandler(
    GuideDbContext dbContext,
    UserManager<User> userManager,
    IHttpContextAccessor httpContextAccessor)
    : IRequestHandler<MakeReviewCommand>
{
    public async Task Handle(MakeReviewCommand request, CancellationToken cancellationToken)
    {
        var claimsPrincipal = httpContextAccessor.HttpContext?.User;
        if (claimsPrincipal == null) return;

        var user = await userManager.GetUserAsync(claimsPrincipal);
        if (user == null) return;

        if (request.ReviewId == 0)
        {
            var attractionId = await dbContext.Attractions
                .Where(x => x.Id == request.AttractionId)
                .Select(x => x.Id)
                .FirstOrDefaultAsync(cancellationToken);
            if (attractionId == 0) return;

            await dbContext.Reviews.AddAsync(new Review
            {
                AttractionId = attractionId, UserId = user.Id,
                Comment = request.Comment, Rating = request.Rating > 5 ? (byte)5 : request.Rating
            }, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);
            return;
        }

        var review = await dbContext.Reviews.FirstOrDefaultAsync(x => x.Id == request.ReviewId, cancellationToken);
        if (review == null || review.UserId != user.Id) return;

        review.Comment = request.Comment;
        review.Rating = request.Rating > 5 ? (byte)5 : request.Rating;
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}