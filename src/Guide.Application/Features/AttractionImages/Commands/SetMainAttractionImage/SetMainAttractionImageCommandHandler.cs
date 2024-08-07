﻿using Guide.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Guide.Application.Features.AttractionImages.Commands.SetMainAttractionImage;

public class SetMainAttractionImageCommandHandler(GuideDbContext dbContext)
    : IRequestHandler<SetMainAttractionImageCommand>
{
    public async Task Handle(SetMainAttractionImageCommand request, CancellationToken cancellationToken)
    {
        await dbContext.AttractionImages
            .Where(x => x.AttractionId == request.Attractionid)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.IsMain, false), cancellationToken);

        await dbContext.AttractionImages
            .Where(x => x.Id == request.ImageId)
            .ExecuteUpdateAsync(x => x.SetProperty(y => y.IsMain, true), cancellationToken);
    }
}