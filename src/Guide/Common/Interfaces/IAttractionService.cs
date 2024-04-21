using Guide.Domain.Entities;

namespace Guide.Common.Interfaces;

public interface IAttractionService
{
    Task<Attraction?> Get(int id);
    Task<List<Attraction>> GetAll();
    Task Create(Attraction attraction);
    Task Update(Attraction attraction);
    Task Delete(int id);
    Task<int> GetCount();
    Task<List<Attraction>> GetPage(int page, int pageSize);
}