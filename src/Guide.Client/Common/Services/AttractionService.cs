using System.Net.Http.Json;
using Guide.Shared.Common.Dtos;
using Guide.Shared.Common.Interfaces;

namespace Guide.Client.Common.Services;

public class AttractionService(HttpClient httpClient) : IAttractionService
{
    public async Task<AttractionDto?> GetById(int id)
    {
        return await httpClient.GetFromJsonAsync<AttractionDto?>($"api/attraction/{id}");
    }

    public async Task<List<AttractionDto>> Get(int page = 0, int limit = 0, string? orderBy = null,
        string? search = null)
    {
        return await httpClient.GetFromJsonAsync<List<AttractionDto>>(
            $"api/attraction/?page={page}&limit={limit}&orderBy={orderBy}&search={search}") ?? [];
    }

    public Task<int> GetCount(string? search = null)
    {
        throw new NotImplementedException();
    }
}