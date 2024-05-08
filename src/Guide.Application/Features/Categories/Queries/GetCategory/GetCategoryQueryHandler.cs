using AutoMapper;
using Guide.Infrastructure;
using Guide.Shared.Common.Dtos;
using Guide.Shared.Common.Static;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Guide.Application.Features.Categories.Queries.GetCategory;

public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryDto?>
{
    private readonly GuideDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetCategoryQueryHandler(GuideDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<CategoryDto?> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var lang = request.LanguageCode ?? LanguageCodes.Default;

        var attraction = await _dbContext.Categories
            .Include(x => x.Translations.Where(x => x.LanguageCode == lang))
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return _mapper.Map<CategoryDto?>(attraction);
    }
}