using FluentValidation;
using Guide.Shared.Common.Static;

namespace Guide.Application.Features.Attractions.Queries.GetAttraction;

public class GetAttractionQueryValidator : AbstractValidator<GetAttractionQuery>
{
    public GetAttractionQueryValidator()
    {
        When(x => !string.IsNullOrEmpty(x.LanguageCode), () =>
        {
            RuleFor(x => x.LanguageCode).Must(x => LanguageCodes.List.Contains(x));
        });
    }
}