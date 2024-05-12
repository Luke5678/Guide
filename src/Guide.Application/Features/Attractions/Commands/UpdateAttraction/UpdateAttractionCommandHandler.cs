using AutoMapper;
using Guide.Infrastructure;
using Guide.Shared.Common.Dtos;
using Guide.Shared.Common.Static;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Guide.Application.Features.Attractions.Commands.UpdateAttraction;

public class UpdateAttractionCommandHandler : IRequestHandler<UpdateAttractionCommand, AttractionDto?>
{
    private readonly GuideDbContext _dbContext;
    private readonly IMapper _mapper;

    public UpdateAttractionCommandHandler(GuideDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<AttractionDto?> Handle(UpdateAttractionCommand request, CancellationToken cancellationToken)
    {
        var lang = request.LanguageCode ?? LanguageCodes.Default;
        
        var attraction = await _dbContext.Attractions
            .Include(x => x.Translations)
            .Include(x => x.Categories)
            .ThenInclude(x => x.Translations.Where(x => x.LanguageCode == lang))
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (attraction == null)
        {
            return null;
        }

        var categories = await _dbContext.Categories
            .Include(x => x.Translations.Where(x => x.LanguageCode == lang))
            .Where(x => request.Categories.Contains(x.Id))
            .ToListAsync(cancellationToken);

        attraction.Translations.First(x => x.LanguageCode == lang).Name = request.Name;
        attraction.Translations.First(x => x.LanguageCode == lang).Description = request.Description;
        attraction.Categories = categories;
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AttractionDto>(attraction);
    }
}