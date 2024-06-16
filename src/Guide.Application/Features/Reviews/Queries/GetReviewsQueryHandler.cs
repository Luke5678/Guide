using Guide.Infrastructure;
using Guide.Shared.Common.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Guide.Application.Features.Reviews.Queries;

public class GetReviewsQueryHandler(GuideDbContext dbContext) : IRequestHandler<GetReviewsQuery, List<ReviewDto>>
{
    public async Task<List<ReviewDto>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
    {
        return await dbContext.Reviews.AsNoTracking()
            .Where(x => x.AttractionId == request.AttractionId)
            .Select(x => new ReviewDto
            {
                Id = x.Id, UserId = x.UserId, Comment = x.Comment, UserName = x.User.UserName!,
                Rating = x.Rating, Submitted = x.Modified!.Value
            })
            .OrderByDescending(x => x.Submitted)
            .ToListAsync(cancellationToken);
    }
}