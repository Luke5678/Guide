using Guide.Application.Features.Categories.Commands.CreateCategory;
using Guide.Application.Features.Categories.Commands.UpdateCategory;
using Guide.Shared.Common.Dtos;

namespace Guide.Application.Common.Interfaces;

public interface ICategoryService
{
    Task Create(CreateCategoryCommand request);
    Task<CategoryDto?> Get(int id);
    Task<List<CategoryDto>> GetAll(int page = 0, int limit = 0, string? orderBy = null, string? search = null);
    Task<CategoryDto?> Update(UpdateCategoryCommand request);
    Task<bool> Delete(int id);
    Task<int> GetCount(string? search = null);
}