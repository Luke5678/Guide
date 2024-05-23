using System.Globalization;
using Guide.Application.Features.Categories.Queries.GetCategories;
using Guide.Application.Features.Categories.Queries.GetCategory;
using Guide.Shared.Common.Dtos;
using Guide.Shared.Common.Interfaces;
using MediatR;

namespace Guide.Application.Common.Services;

public class CategoryService(IMediator mediator) : ICategoryService
{
    public async Task<CategoryDto?> GetById(int id)
    {
        return await mediator.Send(new GetCategoryQuery
        {
            Id = id,
            LanguageCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName
        });
    }

    public async Task<List<CategoryDto>> Get(int page = 0, int limit = 0, string? orderBy = null,
        string? search = null)
    {
        return await mediator.Send(new GetCategoriesQuery
        {
            Page = page, Limit = limit, OrderBy = orderBy, Search = search,
            LanguageCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName
        });
    }
}