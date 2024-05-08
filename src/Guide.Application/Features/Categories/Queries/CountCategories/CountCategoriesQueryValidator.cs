using FluentValidation;
using Guide.Shared.Common.Static;

namespace Guide.Application.Features.Categories.Queries.CountCategories;

public class CountCategoriesQueryValidator : AbstractValidator<CountCategoriesQuery>
{
    public CountCategoriesQueryValidator()
    {
        When(x => !string.IsNullOrEmpty(x.LanguageCode), () =>
        {
            RuleFor(x => x.LanguageCode).Must(x => LanguageCodes.List.Contains(x));
        });
    }
}