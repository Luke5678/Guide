using Guide.Domain.Entities;
using Guide.Shared.Common.Dtos;
using MediatR;

namespace Guide.Application.Features.Attractions.Commands.CreateAttraction;

public class CreateAttractionCommand : IRequest<AttractionDto?>
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public List<int> Categories { get; set; } = [];
    public List<AttractionImage> Images { get; set; } = [];
}