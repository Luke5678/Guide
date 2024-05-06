using Guide.Domain.Entities;
using MediatR;

namespace Guide.Application.Features.Attractions.Commands.CreateAttraction;

public class CreateAttractionCommand : IRequest<Attraction>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public ICollection<int> Categories { get; set; } = [];
}