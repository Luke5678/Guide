using MediatR;

namespace Guide.Application.Features.Reviews.Commands;

public class DeleteReviewCommand : IRequest
{
    public int ReviewId { get; set; }
}