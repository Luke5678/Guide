using Guide.Infrastructure;
using Guide.Shared.Common.Static;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Guide.Application.Features.Categories.Queries.CountCategories;

public class CountCategoriesQueryHandler : IRequestHandler<CountCategoriesQuery, int>
{
    private readonly GuideDbContext _dbContext;

    public CountCategoriesQueryHandler(GuideDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> Handle(CountCategoriesQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Search))
        {
            return await _dbContext.Categories.Select(x => x.Id).CountAsync(cancellationToken);
        }

        var lang = request.LanguageCode ?? LanguageCodes.Default;
        var search = request.Search.ToUpper();

        return await _dbContext.Categories
            .Include(x => x.Translations.Where(x => x.LanguageCode == lang))
            .Select(x => new
            {
                x.Translations.First().Name
            })
            .Where(x => x.Name.ToUpper().Contains(search))
            .AsNoTracking()
            .CountAsync(cancellationToken);
    }
}