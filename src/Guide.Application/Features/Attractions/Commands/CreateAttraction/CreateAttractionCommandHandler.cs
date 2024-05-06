using Guide.Domain.Entities;
using Guide.Infrastructure;
using Guide.Shared.Common.Static;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Guide.Application.Features.Attractions.Commands.CreateAttraction;

public class CreateAttractionCommandHandler : IRequestHandler<CreateAttractionCommand, Attraction>
{
    private readonly GuideDbContext _dbContext;

    public CreateAttractionCommandHandler(GuideDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Attraction> Handle(CreateAttractionCommand request, CancellationToken cancellationToken)
    {
        var categories = await _dbContext.Categories
            .Select(x => new Category { Id = x.Id })
            .Where(x => request.Categories.Contains(x.Id))
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

        return attraction;
    }
}