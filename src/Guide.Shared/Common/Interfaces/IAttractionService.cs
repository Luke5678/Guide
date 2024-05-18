using Guide.Shared.Common.Dtos;

namespace Guide.Shared.Common.Interfaces;

public interface IAttractionService
{
    Task<AttractionDto?> GetById(int id);
    Task<List<AttractionDto>> Get(int page = 0, int limit = 0, string? orderBy = null, string? search = null);
    Task<int> GetCount(string? search = null);
}