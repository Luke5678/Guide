﻿using Guide.Application.Common.Interfaces;
using Guide.Application.Features.Categories.Commands.CreateCategory;
using Guide.Application.Features.Categories.Commands.DeleteCategory;
using Guide.Application.Features.Categories.Commands.UpdateCategory;
using Guide.Application.Features.Categories.Queries.CountCategories;
using Guide.Application.Features.Categories.Queries.GetCategories;
using Guide.Application.Features.Categories.Queries.GetCategory;
using Guide.Infrastructure;
using Guide.Shared.Common.Dtos;
using MediatR;

namespace Guide.Application.Common.Services;

public class CategoryService(GuideDbContext dbContext, IMediator mediator) : ICategoryService
{
    public async Task Create(CreateCategoryCommand request)
    {
        await mediator.Send(request);
    }

    public async Task<CategoryDto?> Get(int id)
    {
        var request = new GetCategoryQuery { Id = id };
        return await mediator.Send(request);
    }

    public async Task<List<CategoryDto>> GetAll(int page = 0, int limit = 0, string? orderBy = null,
        string? search = null)
    {
        var query = new GetCategoriesQuery
        {
            Page = page, Limit = limit, OrderBy = orderBy, Search = search
        };
        return (await mediator.Send(query)).ToList();
    }

    public async Task<CategoryDto?> Update(UpdateCategoryCommand request)
    {
        return await mediator.Send(request);
    }

    public async Task<bool> Delete(int id)
    {
        var request = new DeleteCategoryCommand { Id = id };
        return await mediator.Send(request);
    }

    public async Task<int> GetCount(string? search = null)
    {
        var request = new CountCategoriesQuery { Search = search };
        return await mediator.Send(request);
    }
}