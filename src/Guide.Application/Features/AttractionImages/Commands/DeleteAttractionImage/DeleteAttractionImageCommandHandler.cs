using Guide.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Guide.Application.Features.AttractionImages.Commands.DeleteAttractionImage;

public class DeleteAttractionImageCommandHandler(GuideDbContext dbContext)
    : IRequestHandler<DeleteAttractionImageCommand>
{
    public async Task Handle(DeleteAttractionImageCommand request, CancellationToken cancellationToken)
    {
        var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "uploads", request.Image.Path);
        if (File.Exists(path))
        {
            File.Delete(path);
        }

        await dbContext.AttractionImages
            .Where(x => x.Id == request.Image.Id)
            .ExecuteDeleteAsync(cancellationToken);

        if (request.Image.IsMain)
        {
            await dbContext.AttractionImages
                .Where(x => x.AttractionId == request.Image.AttractionId)
                .ExecuteUpdateAsync(x => x.SetProperty(x => x.IsMain, false), cancellationToken);

            var mainImage = await dbContext.AttractionImages
                .Where(x => x.AttractionId == request.Image.AttractionId)
                .FirstOrDefaultAsync(cancellationToken);

            if (mainImage != null)
            {
                mainImage.IsMain = true;
                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}