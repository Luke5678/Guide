﻿using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Bogus;
using MediatR;
using Guide.Application.Common.Interfaces;
using Guide.Application.Features.Attractions.Commands.CreateAttraction;
using Guide.Application.Features.Attractions.Queries.GetAttraction;
using Guide.Application.Features.Attractions.Queries.GetAttractions;
using Guide.Domain.Entities;
using Guide.Infrastructure;
using Guide.Shared.Common.Dtos;
using Guide.Shared.Common.Static;

namespace Guide.Application.Common.Services;

public class AttractionService(GuideDbContext dbContext, IMediator mediator) : IAttractionService
{
    public async Task Create(CreateAttractionCommand request)
    {
        if (dbContext.Attractions.Count() < 500)
        {
            var faker = new Faker<CreateAttractionCommand>()
                .RuleFor(u => u.Name, (f, u) => f.Commerce.ProductName())
                .RuleFor(u => u.Description, f => f.Lorem.Sentence());

            foreach (var fake in faker.Generate(500))
            {
                await mediator.Send(fake);
            }
        }

        await mediator.Send(request);
    }

    public async Task<AttractionDto> Get(int id)
    {
        var query = new GetAttractionQuery { Id = id };
        return await mediator.Send(query);
    }

    public async Task<List<AttractionDto>> GetAll(int page = 0, int limit = 0, string? orderBy = null,
        string? search = null)
    {
        var query = new GetAttractionsQuery
        {
            Page = page, Limit = limit, OrderBy = orderBy, Search = search
        };
        return (await mediator.Send(query)).ToList();
    }

    public async Task Update(Attraction attraction)
    {
        var dbAttraction = await dbContext.Attractions.FindAsync(attraction.Id);

        if (dbAttraction != null)
        {
            // dbAttraction.Name = attraction.Name;
            // dbAttraction.Category = attraction.Category;
            // dbAttraction.Description = attraction.Description;
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task Delete(int id)
    {
        var dbAttraction = await dbContext.Attractions.FindAsync(id);

        if (dbAttraction != null)
        {
            dbContext.Remove(dbAttraction);
            await dbContext.SaveChangesAsync();
        }
    }

    public async Task<int> GetCount(string? search = null)
    {
        // if (!string.IsNullOrEmpty(search))
        // {
        //     return await dbContext.Attractions
        //         .Where(x => x.Name.Contains(search) || x.Category.Contains(search))
        //         .CountAsync();
        // }

        return await dbContext.Attractions.CountAsync();
    }
}