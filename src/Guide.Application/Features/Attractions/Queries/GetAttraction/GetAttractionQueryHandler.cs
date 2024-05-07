using AutoMapper;
using Guide.Infrastructure;
using Guide.Shared.Common.Dtos;
using Guide.Shared.Common.Static;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Guide.Application.Features.Attractions.Queries.GetAttraction;

public class GetAttractionQueryHandler : IRequestHandler<GetAttractionQuery, AttractionDto>
{
    private readonly GuideDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetAttractionQueryHandler(GuideDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<AttractionDto> Handle(GetAttractionQuery request, CancellationToken cancellationToken)
    {
        var lang = request.LanguageCode ?? LanguageCodes.Default;

        var attraction = await _dbContext.Attractions
            .Include(x => x.Translations.Where(x => x.LanguageCode == lang))
            .Include(x => x.Categories)
            .ThenInclude(x => x.Translations.Where(x => x.LanguageCode == lang))
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        return _mapper.Map<AttractionDto>(attraction);
    }
}