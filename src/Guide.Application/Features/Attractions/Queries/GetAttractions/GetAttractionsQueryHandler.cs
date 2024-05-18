using Guide.Infrastructure;
using Guide.Shared.Common.Dtos;
using Guide.Shared.Common.Static;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Guide.Application.Features.Attractions.Queries.GetAttractions;

public class GetAttractionsQueryHandler(GuideDbContext dbContext)
    : IRequestHandler<GetAttractionsQuery, List<AttractionDto>>
{
    public async Task<List<AttractionDto>> Handle(
        GetAttractionsQuery request,
        CancellationToken cancellationToken)
    {
        var lang = request.LanguageCode ?? LanguageCodes.Default;

        var query = dbContext.Attractions
            .Select(x => new AttractionDto
            {
                Id = x.Id, Name = x.Translations.First(x => x.LanguageCode == lang).Name,
                Categories = x.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id, Name = x.Translations.First(x => x.LanguageCode == lang).Name
                })
            })
            .AsNoTracking();

        if (!string.IsNullOrEmpty(request.Search))
        {
            var search = request.Search.ToUpper();
            query = query.Where(x =>
                x.Name.ToUpper().Contains(search) ||
                x.Categories.Any(x => x.Name.ToUpper().Contains(search)));
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
                "name asc" => query.OrderBy(x => x.Name),
                "name desc" => query.OrderByDescending(x => x.Name),
                "category asc" => query.OrderBy(x => x.Categories.First()),
                "category desc" => query.OrderByDescending(x => x.Categories.First()),
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