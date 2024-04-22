using Guide.Domain.Entities;

namespace Guide.Common.Interfaces;

public interface IAttractionService
{
    Task<Attraction?> Get(int id);
    Task<List<Attraction>> GetAll(int page = 0, int limit = 0, string? orderBy = null);
    Task Create(Attraction attraction);
    Task Update(Attraction attraction);
    Task Delete(int id);
    Task<int> GetCount();
}