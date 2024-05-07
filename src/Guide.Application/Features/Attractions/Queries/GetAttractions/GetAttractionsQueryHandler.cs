using AutoMapper;
using Guide.Infrastructure;
using Guide.Shared.Common.Dtos;
using Guide.Shared.Common.Static;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Guide.Application.Features.Attractions.Queries.GetAttractions;

public class GetAttractionsQueryHandler : IRequestHandler<GetAttractionsQuery, ICollection<AttractionDto>>
{
    private readonly GuideDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetAttractionsQueryHandler(GuideDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<ICollection<AttractionDto>> Handle(
        GetAttractionsQuery request,
        CancellationToken cancellationToken)
    {
        var lang = request.LanguageCode ?? LanguageCodes.Default;

        var query = _dbContext.Attractions
            .Include(x => x.Translations.Where(x => x.LanguageCode == lang))
            .Include(x => x.Categories)
            .ThenInclude(x => x.Translations.Where(x => x.LanguageCode == lang))
            .Select(x => new AttractionDto
            {
                Id = x.Id, Name = x.Translations.Single().Name,
                Categories = x.Categories.Select(x => new CategoryDto
                {
                    Id = x.Id, Name = x.Translations.Single().Name
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