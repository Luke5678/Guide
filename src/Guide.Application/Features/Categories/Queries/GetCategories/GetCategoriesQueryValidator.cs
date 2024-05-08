using FluentValidation;
using Guide.Shared.Common.Static;

namespace Guide.Application.Features.Categories.Queries.GetCategories;

public class GetCategoriesQueryValidator : AbstractValidator<GetCategoriesQuery>
{
    public GetCategoriesQueryValidator()
    {
        When(x => !string.IsNullOrEmpty(x.LanguageCode), () =>
        {
            RuleFor(x => x.LanguageCode).Must(x => LanguageCodes.List.Contains(x));
        });
    }
}