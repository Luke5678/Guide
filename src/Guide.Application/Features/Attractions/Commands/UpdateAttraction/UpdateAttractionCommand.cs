using Guide.Shared.Common.Dtos;
using MediatR;

namespace Guide.Application.Features.Attractions.Commands.UpdateAttraction;

public class UpdateAttractionCommand : IRequest<AttractionDto?>
{
    public int Id { get; set; }

    public string? LanguageCode { get; set; }
    public string Name { get; set; } = null!;
    public string? ShortDescription { get; set; }
    public string Description { get; set; } = null!;
    public IEnumerable<int> Categories { get; set; } = [];
}