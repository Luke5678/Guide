using MediatR;

namespace Guide.Application.Features.Attractions.Commands.DeleteAttraction;

public class DeleteAttractionCommand : IRequest<bool>
{
    public int Id { get; set; }
}