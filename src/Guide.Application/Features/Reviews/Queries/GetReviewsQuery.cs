using Guide.Shared.Common.Dtos;
using MediatR;

namespace Guide.Application.Features.Reviews.Queries;

public class GetReviewsQuery : IRequest<List<ReviewDto>>
{
    public string? LanguageCode { get; set; }
    public int Page { get; set; }
    public int Limit { get; set; }
    public string? OrderBy { get; set; }
    public string? Search { get; set; }
}