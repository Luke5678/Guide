using FluentValidation;

namespace Guide.Application.Features.Reviews.Commands;

public class MakeReviewCommandValidator : AbstractValidator<MakeReviewCommand>
{
    public MakeReviewCommandValidator()
    {
        RuleFor(x => x.Comment).NotEmpty();
    }
}