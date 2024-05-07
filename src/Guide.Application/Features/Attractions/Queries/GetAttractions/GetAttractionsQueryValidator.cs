using FluentValidation;
using Guide.Shared.Common.Static;

namespace Guide.Application.Features.Attractions.Queries.GetAttractions;

public class GetAttractionsQueryValidator : AbstractValidator<GetAttractionsQuery>
{
    public GetAttractionsQueryValidator()
    {
        When(x => !string.IsNullOrEmpty(x.LanguageCode), () =>
        {
            RuleFor(x => x.LanguageCode).Must(x => LanguageCodes.List.Contains(x));
        });
    }
}