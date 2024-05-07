using Guide.Infrastructure;
using Guide.Shared.Common.Static;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Guide.Application.Features.Attractions.Queries.CountAttractions;

public class CountAttractionsQueryHandler : IRequestHandler<CountAttractionsQuery, int>
{
    private readonly GuideDbContext _dbContext;

    public CountAttractionsQueryHandler(GuideDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> Handle(CountAttractionsQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Search))
        {
            return await _dbContext.Attractions.Select(x => x.Id).CountAsync(cancellationToken);
        }

        var lang = request.LanguageCode ?? LanguageCodes.Default;
        var search = request.Search.ToUpper();

        return await _dbContext.Attractions
            .Include(x => x.Translations.Where(x => x.LanguageCode == lang))
            .Include(x => x.Categories)
            .ThenInclude(x => x.Translations.Where(x => x.LanguageCode == lang))
            .Select(x => new
            {
                x.Translations.First().Name,
                Categories = x.Categories.Select(x => x.Translations.First().Name)
            })
            .Where(x =>
                x.Name.ToUpper().Contains(search) ||
                x.Categories.Any(x => x.ToUpper().Contains(search)))
            .AsNoTracking()
            .CountAsync(cancellationToken);
    }
}