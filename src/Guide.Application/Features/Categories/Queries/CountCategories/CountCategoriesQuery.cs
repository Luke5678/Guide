using MediatR;

namespace Guide.Application.Features.Categories.Queries.CountCategories;

public class CountCategoriesQuery : IRequest<int>
{
    public string? LanguageCode { get; set; }
    public string? Search { get; set; }
}