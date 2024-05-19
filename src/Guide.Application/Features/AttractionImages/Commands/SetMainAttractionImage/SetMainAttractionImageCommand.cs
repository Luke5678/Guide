using Guide.Domain.Entities;
using MediatR;

namespace Guide.Application.Features.AttractionImages.Commands.SetMainAttractionImage;

public class SetMainAttractionImageCommand : IRequest
{
    public int ImageId { get; set; }
    public int Attractionid { get; set; }
}