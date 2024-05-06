using AutoMapper;
using Guide.Domain.Entities;
using Guide.Shared.Common.Interfaces;

namespace Guide.Shared.Common.Dtos;

public class CategoryDto : IMapFrom<Category>
{
    public int Id { get; set; }
    public string Name { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Category, CategoryDto>()
            .ForMember(x => x.Name,
                x => x.MapFrom(x => x.Translations.First().Name));
    }
}