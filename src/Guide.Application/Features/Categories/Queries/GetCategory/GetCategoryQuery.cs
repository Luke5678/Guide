using Guide.Shared.Common.Dtos;
using MediatR;

namespace Guide.Application.Features.Categories.Queries.GetCategory;

public class GetCategoryQuery : IRequest<CategoryDto?>
{
    public int Id { get; set; }
    public string? LanguageCode { get; set; }
}