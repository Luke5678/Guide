using FluentValidation;
using Guide.Shared.Common.Static;

namespace Guide.Application.Features.Reviews.Queries;

public class GetReviewsQueryValidator : AbstractValidator<GetReviewsQuery>
{
    public GetReviewsQueryValidator()
    {
        When(x => !string.IsNullOrEmpty(x.LanguageCode), () =>
        {
            RuleFor(x => x.LanguageCode).Must(x => LanguageCodes.List.Contains(x));
        });
    }
}