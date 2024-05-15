using System.Globalization;
using Bogus;
using MediatR;
using Guide.Application.Common.Interfaces;
using Guide.Application.Features.AttractionImages.Commands.AddAttractionImages;
using Guide.Application.Features.AttractionImages.Commands.DeleteAttractionImage;
using Guide.Application.Features.AttractionImages.Commands.SetMainAttractionImage;
using Guide.Application.Features.Attractions.Commands.CreateAttraction;
using Guide.Application.Features.Attractions.Commands.DeleteAttraction;
using Guide.Application.Features.Attractions.Commands.UpdateAttraction;
using Guide.Application.Features.Attractions.Queries.CountAttractions;
using Guide.Application.Features.Attractions.Queries.GetAttraction;
using Guide.Application.Features.Attractions.Queries.GetAttractions;
using Guide.Domain.Entities;
using Guide.Infrastructure;
using Guide.Shared.Common.Dtos;
using Microsoft.AspNetCore.Components.Forms;

namespace Guide.Application.Common.Services;

public class AttractionService(GuideDbContext dbContext, IMediator mediator)
    : IAttractionService
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
        return await mediator.Send(new DeleteAttractionCommand { Id = id });
    }

    public async Task<int> GetCount(string? search = null)
    {
        return await mediator.Send(new CountAttractionsQuery { Search = search });
    }

    public async Task<List<AttractionImage>> AddImages(IReadOnlyList<IBrowserFile> files, int attractionId = 0)
    {
        return await mediator.Send(new AddAttractionImagesCommand
        {
            Files = files, AttractionId = attractionId
        });
    }

    public async Task DeleteImage(AttractionImage image)
    {
        await mediator.Send(new DeleteAttractionImageCommand { Image = image });
    }

    public async Task SetImageAsMain(AttractionImage image)
    {
        await mediator.Send(new SetMainAttractionImageCommand { Image = image });
    }
}