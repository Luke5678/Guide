﻿using Guide.Infrastructure;
using Guide.Shared.Common.Dtos;
using Guide.Shared.Common.Static;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Guide.Application.Features.Attractions.Queries.GetAttractions;

public class GetAttractionsQueryHandler(IDbContextFactory<GuideDbContext> dbContextFactory)
    : IRequestHandler<GetAttractionsQuery, List<AttractionDto>>
{
    public async Task<List<AttractionDto>> Handle(
        GetAttractionsQuery request,
        CancellationToken cancellationToken)
    {
        var lang = request.LanguageCode ?? LanguageCodes.Default;
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        if (request.Limit is > 100 or < 1)
        {
            request.Limit = 20;
        }

        var query = dbContext.Attractions
            .Select(x => new AttractionDto
            {
                Id = x.Id, 
                Name = x.Translations.First(y => y.LanguageCode == lang).Name,
                ShortDescription = x.Translations.First(y => y.LanguageCode == lang).ShortDescription,
                Images = x.Images.Select(y => new AttractionImageDto
                {
                    Url = y.Url, IsMain = y.IsMain
                }),
                Categories = x.Categories.Select(y => new CategoryDto
                {
                    Id = y.Id, Name = y.Translations.First(z => z.LanguageCode == lang).Name
                })
            })
            .AsNoTracking();

        if (request.Categories.Length > 0)
        {
            query = query.Where(x => x.Categories.Any(y => request.Categories.Contains(y.Id)));
        }

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
        else
        {
            query = query.Take(request.Limit);
        }

        return await query.ToListAsync(cancellationToken);
    }
}