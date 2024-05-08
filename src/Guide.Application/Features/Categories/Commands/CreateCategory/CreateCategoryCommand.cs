using Guide.Shared.Common.Dtos;
using MediatR;

namespace Guide.Application.Features.Categories.Commands.CreateCategory;

public class CreateCategoryCommand : IRequest<CategoryDto?>
{
    public string Name { get; set; } = null!;
}