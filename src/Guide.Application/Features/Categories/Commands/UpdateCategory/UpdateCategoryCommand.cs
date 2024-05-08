using Guide.Shared.Common.Dtos;
using MediatR;

namespace Guide.Application.Features.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommand : IRequest<CategoryDto?>
{
    public int Id { get; set; }
    public string? LanguageCode { get; set; }
    public string Name { get; set; } = null!;
}