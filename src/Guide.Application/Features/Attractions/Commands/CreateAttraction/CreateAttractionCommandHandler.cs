using AutoMapper;
using Guide.Domain.Entities;
using Guide.Infrastructure;
using Guide.Shared.Common.Dtos;
using Guide.Shared.Common.Static;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Guide.Application.Features.Attractions.Commands.CreateAttraction;

public class CreateAttractionCommandHandler : IRequestHandler<CreateAttractionCommand, AttractionDto?>
{
    private readonly GuideDbContext _dbContext;
    private readonly IMapper _mapper;

    public CreateAttractionCommandHandler(GuideDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<AttractionDto?> Handle(CreateAttractionCommand request, CancellationToken cancellationToken)
    {
        var lang = LanguageCodes.Default;
        
        var categories = await _dbContext.Categories
            .Include(x => x.Translations.Where(x => x.LanguageCode == lang))
            .AsNoTracking()
            .ToListAsync(cancellationToken: cancellationToken);

        var attraction = new Attraction
        {
            Categories = categories,
            Translations = LanguageCodes.List.Select(x => new AttractionTranslation
            {
                LanguageCode = x,
                Name = request.Name,
                Description = request.Description
            }).ToArray()
        };

        await _dbContext.Attractions.AddAsync(attraction, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AttractionDto>(attraction);
    }
}