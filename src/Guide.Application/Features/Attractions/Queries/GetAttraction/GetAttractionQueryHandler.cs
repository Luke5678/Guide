using AutoMapper;
using Guide.Infrastructure;
using Guide.Shared.Common.Dtos;
using Guide.Shared.Common.Static;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Guide.Application.Features.Attractions.Queries.GetAttraction;

public class GetAttractionQueryHandler(GuideDbContext dbContext, IMapper mapper)
    : IRequestHandler<GetAttractionQuery, AttractionDto?>
{
    public async Task<AttractionDto?> Handle(GetAttractionQuery request, CancellationToken cancellationToken)
    {
        var lang = request.LanguageCode ?? LanguageCodes.Default;

        var attraction = await dbContext.Attractions
            .Include(x => x.Categories)
            .ThenInclude(x => x.Translations.Where(y => y.LanguageCode == lang))
            .Include(x => x.Images)
            .Include(x => x.Translations.Where(y => y.LanguageCode == lang))
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return mapper.Map<AttractionDto>(attraction);
    }
}