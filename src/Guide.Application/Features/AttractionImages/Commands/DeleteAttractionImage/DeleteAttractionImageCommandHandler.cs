using Guide.Application.Common.Interfaces;
using Guide.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Guide.Application.Features.AttractionImages.Commands.DeleteAttractionImage;

public class DeleteAttractionImageCommandHandler(GuideDbContext dbContext, IUploadService uploadService)
    : IRequestHandler<DeleteAttractionImageCommand>
{
    public async Task Handle(DeleteAttractionImageCommand request, CancellationToken cancellationToken)
    {
        var image = await dbContext.AttractionImages
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (image == null) return;

        await uploadService.DeleteFileAsync(image.Url);

        await dbContext.AttractionImages
            .Where(x => x.Id == image.Id)
            .ExecuteDeleteAsync(cancellationToken);

        if (image.IsMain)
        {
            await dbContext.AttractionImages
                .Where(x => x.AttractionId == image.AttractionId)
                .ExecuteUpdateAsync(x => x.SetProperty(y => y.IsMain, false), cancellationToken);

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