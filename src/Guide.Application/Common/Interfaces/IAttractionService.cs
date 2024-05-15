using Guide.Application.Features.Attractions.Commands.CreateAttraction;
using Guide.Application.Features.Attractions.Commands.UpdateAttraction;
using Guide.Domain.Entities;
using Guide.Shared.Common.Dtos;
using Microsoft.AspNetCore.Components.Forms;

namespace Guide.Application.Common.Interfaces;

public interface IAttractionService
{
    Task Create(CreateAttractionCommand request);
    Task<AttractionDto?> Get(int id);
    Task<List<AttractionDto>> GetAll(int page = 0, int limit = 0, string? orderBy = null, string? search = null);
    Task<AttractionDto?> Update(UpdateAttractionCommand request);
    Task<bool> Delete(int id);
    Task<int> GetCount(string? search = null);
    Task<List<AttractionImage>> AddImages(IReadOnlyList<IBrowserFile> files, int attractionId = 0);
    Task DeleteImage(AttractionImage images);
    Task SetImageAsMain(AttractionImage images);
}