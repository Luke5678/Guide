using MediatR;

namespace Guide.Application.Features.Reviews.Commands;

public class MakeReviewCommand : IRequest
{
    public int AttractionId { get; set; }
    public int ReviewId { get; set; }
    public required string Comment { get; set; }
    public required byte Rating { get; set; }
}