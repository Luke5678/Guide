using AutoMapper;
using Guide.Infrastructure;
using Guide.Shared.Common.Dtos;
using Guide.Shared.Common.Static;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Guide.Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, ICollection<CategoryDto>>
{
    private readonly GuideDbContext _dbContext;

    public GetCategoriesQueryHandler(GuideDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ICollection<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var lang = request.LanguageCode ?? LanguageCodes.Default;

        var query = _dbContext.Categories
            .Include(x => x.Translations.Where(x => x.LanguageCode == lang))
            .Select(x => new CategoryDto
            {
                Id = x.Id, Name = x.Translations.Single().Name
            })
            .AsNoTracking();

        if (!string.IsNullOrEmpty(request.Search))
        {
            var search = request.Search.ToUpper();
            query = query.Where(x => x.Name.ToUpper().Contains(search));
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