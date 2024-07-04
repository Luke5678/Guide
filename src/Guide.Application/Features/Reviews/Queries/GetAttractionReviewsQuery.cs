using Guide.Shared.Common.Dtos;
using MediatR;

namespace Guide.Application.Features.Reviews.Queries;

public class GetAttractionReviewsQuery : IRequest<List<ReviewDto>>
{
    public required int AttractionId { get; set; }
}