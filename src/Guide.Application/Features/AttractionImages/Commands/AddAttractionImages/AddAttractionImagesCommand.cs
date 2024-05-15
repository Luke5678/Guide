using Guide.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;

namespace Guide.Application.Features.AttractionImages.Commands.AddAttractionImages;

public class AddAttractionImagesCommand : IRequest<List<AttractionImage>>
{
    public IReadOnlyList<IBrowserFile> Files { get; set; } = [];
    public int AttractionId { get; set; }
}