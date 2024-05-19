using Guide.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Guide.Application.Features.AttractionImages.Commands.DeleteAttractionImage;

public class DeleteAttractionImageCommandHandler(GuideDbContext dbContext)
    : IRequestHandler<DeleteAttractionImageCommand>
{
    public async Task Handle(DeleteAttractionImageCommand request, CancellationToken cancellationToken)
    {
        var image = await dbContext.AttractionImages
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (image == null) return;

        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "uploads", image.Path);
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        await dbContext.AttractionImages
            .Where(x => x.Id == image.Id)
            .ExecuteDeleteAsync(cancellationToken);

        if (image.IsMain)
        {
            await dbContext.AttractionImages
                .Where(x => x.AttractionId == image.AttractionId)
                .ExecuteUpdateAsync(x => x.SetProperty(x => x.IsMain, false), cancellationToken);

            var mainImage = await dbContext.AttractionImages
                .Where(x => x.AttractionId == image.AttractionId)
                .FirstOrDefaultAsync(cancellationToken);

            if (mainImage != null)
            {
                mainImage.IsMain = true;
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}