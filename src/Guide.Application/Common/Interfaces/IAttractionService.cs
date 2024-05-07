using Guide.Application.Features.Attractions.Commands.CreateAttraction;
using Guide.Domain.Entities;
using Guide.Shared.Common.Dtos;

namespace Guide.Application.Common.Interfaces;

public interface IAttractionService
{
    Task Create(CreateAttractionCommand request);
    Task<AttractionDto> Get(int id);
    Task<List<AttractionDto>> GetAll(int page = 0, int limit = 0, string? orderBy = null, string? search = null);
    Task Update(Attraction attraction);
    Task Delete(int id);
    Task<int> GetCount(string? search = null);
}