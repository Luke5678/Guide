using FluentValidation;
using Guide.Shared.Common.Static;

namespace Guide.Application.Features.Attractions.Queries.CountAttractions;

public class CountAttractionsQueryValidator : AbstractValidator<CountAttractionsQuery>
{
    public CountAttractionsQueryValidator()
    {
        When(x => !string.IsNullOrEmpty(x.LanguageCode), () =>
        {
            RuleFor(x => x.LanguageCode).Must(x => LanguageCodes.List.Contains(x));
        });
    }
}