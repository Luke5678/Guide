using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Bogus;
using MediatR;
using Guide.Application.Common.Interfaces;
using Guide.Application.Features.Attractions.Commands.CreateAttraction;
using Guide.Domain.Entities;
using Guide.Infrastructure;
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

    public async Task<Attraction?> Get(int id)
    {
        return await dbContext.Attractions.FindAsync(id);
    }

    public async Task<List<Attraction>> GetAll(int page = 0, int limit = 0, string? orderBy = null,
        string? search = null)
    {
        var query = dbContext.Attractions.AsNoTracking();
        var rg = new Regex(@"^\w+ (asc|desc)$", RegexOptions.IgnoreCase);

        // if (!string.IsNullOrEmpty(search))
        // {
        //     query = query.Where(x => x.Name.Contains(search) || x.Category.Contains(search));
        // }
        //
        // if (string.IsNullOrEmpty(orderBy) || !rg.IsMatch(orderBy))
        // {
        //     query = query.OrderBy(x => x.Id);
        // }
        // else
        // {
        //     query = orderBy.ToLower() switch
        //     {
        //         "id asc" => query.OrderBy(x => x.Id),
        //         "id desc" => query.OrderByDescending(x => x.Id),
        //         "name asc" => query.OrderBy(x => x.Name),
        //         "name desc" => query.OrderByDescending(x => x.Name),
        //         "category asc" => query.OrderBy(x => x.Category),
        //         "category desc" => query.OrderByDescending(x => x.Category),
        //         _ => query
        //     };
        // }
        //
        // if (page > 0 && limit > 0)
        // {
        //     query = query.Skip((page - 1) * limit).Take(limit);
        // }

        return await query.ToListAsync();
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