using Guide.Shared.Common.Dtos;

namespace Guide.Shared.Common.Interfaces;

public interface ICategoryService
{
    Task<CategoryDto?> GetById(int id);
    Task<List<CategoryDto>> Get(int page = 0, int limit = 0, string? orderBy = null, string? search = null);
}