using Guide.Shared.Common.Dtos;
using MediatR;

namespace Guide.Application.Features.Attractions.Queries.GetAttractions;

public class GetAttractionsQuery : IRequest<List<AttractionDto>>
{
    public string? LanguageCode { get; set; }
    public int Page { get; set; }
    public int Limit { get; set; }
    public string? OrderBy { get; set; }
    public string? Search { get; set; }
}