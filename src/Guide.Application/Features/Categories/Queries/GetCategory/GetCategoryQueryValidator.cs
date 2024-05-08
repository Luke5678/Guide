using FluentValidation;
using Guide.Shared.Common.Static;

namespace Guide.Application.Features.Categories.Queries.GetCategory;

public class GetCategoryQueryValidator : AbstractValidator<GetCategoryQuery>
{
    public GetCategoryQueryValidator()
    {
        When(x => !string.IsNullOrEmpty(x.LanguageCode), () =>
        {
            RuleFor(x => x.LanguageCode).Must(x => LanguageCodes.List.Contains(x));
        });
    }
}