using Guide.Shared.Common.Dtos;
using Guide.Shared.Common.Interfaces;

namespace Guide.Shared.Common.Services;

public class AttractionService : IAttractionService
{
    public Task<AttractionDto?> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<AttractionDto>> Get(int page = 0, int limit = 0, string? orderBy = null, string? search = null)
    {
        throw new NotImplementedException();
    }

    public Task<int> GetCount(string? search = null)
    {
        throw new NotImplementedException();
    }
}