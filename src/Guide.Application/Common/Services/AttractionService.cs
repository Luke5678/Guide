using System.Globalization;
using Bogus;
using MediatR;
using Guide.Application.Common.Interfaces;
using Guide.Application.Features.Attractions.Commands.CreateAttraction;
using Guide.Application.Features.Attractions.Commands.DeleteAttraction;
using Guide.Application.Features.Attractions.Commands.UpdateAttraction;
using Guide.Application.Features.Attractions.Queries.CountAttractions;
using Guide.Application.Features.Attractions.Queries.GetAttraction;
using Guide.Application.Features.Attractions.Queries.GetAttractions;
using Guide.Infrastructure;
using Guide.Shared.Common.Dtos;

namespace Guide.Application.Common.Services;

public class AttractionService(GuideDbContext dbContext, IMediator mediator) : IAttractionService
{
    private readonly string _language = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;

    public async Task Create(CreateAttractionCommand request)
    {
        if (dbContext.Attractions.Count() < 500)
        {
            var faker = new Faker<CreateAttractionCommand>()
                .RuleFor(u => u.Name, (f, u) => f.Commerce.ProductName())
                .RuleFor(u => u.Description, f => f.Lorem.Sentence());

            foreach (var fake in faker.Generate(500 - dbContext.Attractions.Count()))
            {
                await mediator.Send(fake);
            }
        }

        await mediator.Send(request);
    }

    public async Task<AttractionDto?> Get(int id)
    {
        var request = new GetAttractionQuery { Id = id, LanguageCode = _language };
        return await mediator.Send(request);
    }

    public async Task<List<AttractionDto>> GetAll(int page = 0, int limit = 0, string? orderBy = null,
        string? search = null)
    {
        var query = new GetAttractionsQuery
        {
            Page = page, Limit = limit, OrderBy = orderBy, Search = search, LanguageCode = _language
        };
        return (await mediator.Send(query)).ToList();
    }

    public async Task<AttractionDto?> Update(UpdateAttractionCommand request)
    {
        request.LanguageCode ??= _language;
        return await mediator.Send(request);
    }

    public async Task<bool> Delete(int id)
    {
        var request = new DeleteAttractionCommand { Id = id };
        return await mediator.Send(request);
    }

    public async Task<int> GetCount(string? search = null)
    {
        var request = new CountAttractionsQuery { Search = search };
        return await mediator.Send(request);
    }
}