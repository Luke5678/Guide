using Guide.Shared.Common.Dtos;
using MediatR;

namespace Guide.Application.Features.Attractions.Queries.GetAttraction;

public class GetAttractionQuery : IRequest<AttractionDto>
{
    public int Id { get; set; }
    public string? LanguageCode { get; set; }
}