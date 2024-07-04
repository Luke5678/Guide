using Guide.Infrastructure;
using Guide.Shared.Common.Dtos;
using Guide.Shared.Common.Static;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Guide.Application.Features.Reviews.Queries;

public class GetReviewsQueryHandler(IDbContextFactory<GuideDbContext> dbContextFactory)
    : IRequestHandler<GetReviewsQuery, List<ReviewDto>>
{
    public async Task<List<ReviewDto>> Handle(GetReviewsQuery request, CancellationToken cancellationToken)
    {
        var lang = request.LanguageCode ?? LanguageCodes.Default;
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var query = dbContext.Reviews
            .Select(x => new ReviewDto()
            {
                Id = x.Id, UserId = x.UserId, UserName = x.User.UserName ?? "",
                Comment = x.Comment, Submitted = x.Modified!.Value
            })
            .AsNoTracking();

        if (!string.IsNullOrEmpty(request.Search))
        {
            var search = request.Search.ToUpper();
            query = query.Where(x => x.UserName.ToUpper().Contains(search) ||
                                     x.Comment.ToUpper().Contains(search));
        }

        if (string.IsNullOrEmpty(request.OrderBy))
        {
            query = query.OrderByDescending(x => x.Submitted);
        }
        else
        {
            query = request.OrderBy.ToLower() switch
            {
                "id asc" => query.OrderBy(x => x.Id),
                "id desc" => query.OrderByDescending(x => x.Id),
                "username asc" => query.OrderBy(x => x.UserName),
                "username desc" => query.OrderByDescending(x => x.UserName),
                "comment asc" => query.OrderBy(x => x.Comment),
                "comment desc" => query.OrderByDescending(x => x.Comment),
                "date asc" => query.OrderBy(x => x.Submitted),
                "date desc" => query.OrderByDescending(x => x.Submitted),
                _ => query.OrderByDescending(x => x.Submitted)
            };
        }

        if (request.Page > 0 && request.Limit > 0)
        {
            query = query.Skip((request.Page - 1) * request.Limit).Take(request.Limit);
        }

        return await query.ToListAsync(cancellationToken);
    }
}