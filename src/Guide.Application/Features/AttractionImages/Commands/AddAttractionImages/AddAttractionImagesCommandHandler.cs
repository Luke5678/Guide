using Guide.Domain.Entities;
using Guide.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Guide.Application.Features.AttractionImages.Commands.AddAttractionImages;

public class AddAttractionImagesCommandHandler(GuideDbContext dbContext, IWebHostEnvironment env)
    : IRequestHandler<AddAttractionImagesCommand, List<AttractionImage>>
{
    public async Task<List<AttractionImage>> Handle(AddAttractionImagesCommand request,
        CancellationToken cancellationToken)
    {
        if (!request.Files.Any()) return [];

        const int maxFileSize = 1024 * 1024 * 50; // 50 MB

        var guid = Guid.NewGuid().ToString();
        var basePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "uploads", guid);
        var paths = new List<string>();

        if (env.IsDevelopment())
        {
            basePath = basePath.Replace(@"bin\Debug\net8.0\", "");
        }

        Directory.CreateDirectory(basePath);

        foreach (var file in request.Files)
        {
            try
            {
                var path = Path.Combine(basePath, file.Name);
                await using FileStream fs = new(path, FileMode.Create);
                await file.OpenReadStream(maxFileSize, cancellationToken).CopyToAsync(fs, cancellationToken);

                paths.Add($"{guid}/{file.Name}");
            }
            catch (Exception ex)
            {
                Log.Information($"File: {file.Name} Error: {ex.Message}");
            }
        }

        if (request.AttractionId > 0)
        {
            var attraction = dbContext.Attractions
                .Include(x => x.Images)
                .First(x => x.Id == request.AttractionId);

            foreach (var path in paths)
            {
                attraction.Images.Add(new AttractionImage
                {
                    Path = path
                });
            }

            if (!attraction.Images.Any(x => x.IsMain))
            {
                attraction.Images.First().IsMain = true;
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            return attraction.Images.ToList();
        }

        var images = new List<AttractionImage>();

        foreach (var path in paths)
        {
            var image = new AttractionImage
            {
                Path = path
            };

            images.Add(image);
            dbContext.AttractionImages.Add(image);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return images;
    }
}