using Guide.Domain.Entities;
using MediatR;

namespace Guide.Application.Features.AttractionImages.Commands.DeleteAttractionImage;

public class DeleteAttractionImageCommand : IRequest
{
    public AttractionImage Image { get; set; } = null!;
}