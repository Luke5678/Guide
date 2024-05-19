using AutoMapper;
using Guide.Application.Common.Services;
using Guide.Domain.Entities;
using Guide.Infrastructure;
using Guide.Shared.Common.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Guide.Application.Features.AttractionImages.Commands.AddAttractionImages;

public class AddAttractionImagesCommandHandler(GuideDbContext dbContext, IMapper mapper, BlobService blobService)
    : IRequestHandler<AddAttractionImagesCommand, List<AttractionImageDto>>
{
    public async Task<List<AttractionImageDto>> Handle(AddAttractionImagesCommand request,
        CancellationToken cancellationToken)
    {
        if (!request.Files.Any()) return [];

        const int maxFileSize = 1024 * 1024 * 50; // 50 MB

        var guid = Guid.NewGuid().ToString();
        var urls = new List<string>();

        foreach (var file in request.Files)
        {
            try
            {
                var stream = file.OpenReadStream(maxFileSize, cancellationToken);
                var url = await blobService.UploadFileAsync(stream, $"{guid}/{file.Name}");
                urls.Add(url);
            }
            catch (Exception ex)
            {
                Log.Information($"AttractionImage: {file.Name} Error: {ex.Message}");
            }
        }

        if (request.AttractionId > 0)
        {
            var attraction = dbContext.Attractions
                .Include(x => x.Images)
                .First(x => x.Id == request.AttractionId);

            foreach (var url in urls)
            {
                attraction.Images.Add(new AttractionImage
                {
                    Url = url
                });
            }

            if (!attraction.Images.Any(x => x.IsMain))
            {
                attraction.Images.First().IsMain = true;
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            return mapper.Map<List<AttractionImageDto>>(attraction.Images.ToList());
        }

        var images = new List<AttractionImage>();

        foreach (var url in urls)
        {
            var image = new AttractionImage
            {
                Url = url
            };

            images.Add(image);
            dbContext.AttractionImages.Add(image);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<List<AttractionImageDto>>(images);
    }
}