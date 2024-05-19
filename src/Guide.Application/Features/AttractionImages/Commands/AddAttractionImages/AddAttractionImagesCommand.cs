using Guide.Shared.Common.Dtos;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;

namespace Guide.Application.Features.AttractionImages.Commands.AddAttractionImages;

public class AddAttractionImagesCommand : IRequest<List<AttractionImageDto>>
{
    public IReadOnlyList<IBrowserFile> Files { get; set; } = [];
    public int AttractionId { get; set; }
}