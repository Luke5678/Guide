using System.Text.RegularExpressions;
using Bogus;
using Guide.Common.Interfaces;
using Guide.Domain.Entities;
using Guide.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Guide.Common.Services;

public class AttractionService(GuideDbContext dbContext) : IAttractionService
{
    public async Task<Attraction?> Get(int id)
    {
        return await dbContext.Attractions.FindAsync(id);
    }

    public async Task<List<Attraction>> GetAll(int page = 0, int limit = 0, string? orderBy = null)
    {
        var query = dbContext.Attractions.AsNoTracking();
        var rg = new Regex(@"^\w+ (asc|desc)$", RegexOptions.IgnoreCase);

        if (string.IsNullOrEmpty(orderBy) || !rg.IsMatch(orderBy))
        {
            query = query.OrderBy(x => x.Id);
        }
        else
        {
            query = orderBy.ToLower() switch
            {
                "id asc" => query.OrderBy(x => x.Id),
                "id desc" => query.OrderByDescending(x => x.Id),
                "name asc" => query.OrderBy(x => x.Name),
                "name desc" => query.OrderByDescending(x => x.Name),
                "category asc" => query.OrderBy(x => x.Category),
                "category desc" => query.OrderByDescending(x => x.Category),
                _ => query
            };
        }

        if (page > 0 && limit > 0)
        {
            query = query.Skip((page - 1) * limit).Take(limit);
        }

        return await query.ToListAsync();
    }

    public async Task Create(Attraction attraction)
    {
        if (dbContext.Attractions.Count() < 500)
        {
            var faker = new Faker<Attraction>()
                .RuleFor(u => u.Name, (f, u) => f.Commerce.ProductName())
                .RuleFor(u => u.Category, (f, u) => f.Commerce.Department())
                .RuleFor(u => u.Description, f => f.Lorem.Sentence());

            await dbContext.AddRangeAsync(faker.Generate(500));
        }
        else
        {
            await dbContext.AddAsync(attraction);
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task Update(Attraction attraction)
    {
        var dbAttraction = await dbContext.Attractions.FindAsync(attraction.Id);

        if (dbAttraction != null)
        {
            dbAttraction.Name = attraction.Name;
            dbAttraction.Category = attraction.Category;
            dbAttraction.Description = attraction.Description;
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

    public async Task<int> GetCount()
    {
        return await dbContext.Attractions.Select(x => x.Id).CountAsync();
    }

    public async Task<List<Attraction>> GetPage(int page, int pageSize)
    {
        return await dbContext.Attractions.AsNoTracking()
            .OrderBy(x => x.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }
}