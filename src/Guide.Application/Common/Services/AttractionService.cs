using System.Globalization;
using Guide.Application.Features.Attractions.Queries.GetAttraction;
using Guide.Application.Features.Attractions.Queries.GetAttractions;
using Guide.Shared.Common.Dtos;
using Guide.Shared.Common.Interfaces;
using MediatR;

namespace Guide.Application.Common.Services;

public class AttractionService(IMediator mediator) : IAttractionService
{
    public async Task<AttractionDto?> GetById(int id)
    {
        return await mediator.Send(new GetAttractionQuery
        {
            Id = id,
            LanguageCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName
        });
    }

    public async Task<List<AttractionDto>> Get(int page = 0, int limit = 0, string? orderBy = null,
        string? search = null, int[]? categories = null)
    {
        return await mediator.Send(new GetAttractionsQuery
        {
            Page = page, Limit = limit, OrderBy = orderBy, Search = search,
            Categories = categories ?? [],
            LanguageCode = CultureInfo.CurrentCulture.TwoLetterISOLanguageName
        });
    }

    public Task<int> GetCount(string? search = null)
    {
        throw new NotImplementedException();
    }
}