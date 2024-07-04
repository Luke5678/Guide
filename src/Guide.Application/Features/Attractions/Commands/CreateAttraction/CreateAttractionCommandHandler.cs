using AutoMapper;
using Guide.Domain.Entities;
using Guide.Infrastructure;
using Guide.Shared.Common.Dtos;
using Guide.Shared.Common.Static;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Guide.Application.Features.Attractions.Commands.CreateAttraction;

public class CreateAttractionCommandHandler(GuideDbContext dbContext, IMapper mapper)
    : IRequestHandler<CreateAttractionCommand, AttractionDto?>
{
    public async Task<AttractionDto?> Handle(CreateAttractionCommand request, CancellationToken cancellationToken)
    {
        var categories = await dbContext.Categories
            .Include(x => x.Translations)
            .Where(x => request.Categories.Contains(x.Id))
            .ToListAsync(cancellationToken: cancellationToken);

        var attraction = new Attraction
        {
            Categories = categories,
            Translations = LanguageCodes.List.Select(x => new AttractionTranslation
            {
                LanguageCode = x,
                Name = request.Name,
                ShortDescription = request.ShortDescription,
                Description = request.Description
            }).ToArray()
        };

        await dbContext.Attractions.AddAsync(attraction, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        if (request.Images.Count > 0)
        {
            if (!request.Images.Any(x => x.IsMain))
            {
                request.Images.First().IsMain = true;
            }

            foreach (var image in request.Images)
            {
                var dbImage = await dbContext.AttractionImages.FindAsync(image.Id, cancellationToken);
                if (dbImage != null)
                {
                    dbImage.AttractionId = attraction.Id;
                    dbImage.IsMain = image.IsMain;
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);
        }

        return mapper.Map<AttractionDto>(attraction);
    }
}