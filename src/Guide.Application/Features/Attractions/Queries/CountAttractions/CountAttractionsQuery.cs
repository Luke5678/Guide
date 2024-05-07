using MediatR;

namespace Guide.Application.Features.Attractions.Queries.CountAttractions;

public class CountAttractionsQuery : IRequest<int>
{
    public string? LanguageCode { get; set; }
    public string? Search { get; set; }
}