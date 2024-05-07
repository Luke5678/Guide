using AutoMapper;
using Guide.Domain.Entities;
using Guide.Shared.Common.Interfaces;

namespace Guide.Shared.Common.Dtos;

public class AttractionDto : IMapFrom<Attraction>
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public IEnumerable<CategoryDto> Categories { get; set; } = [];

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Attraction, AttractionDto>()
            .ForMember(x => x.Name, x => x.MapFrom(x => x.Translations.First().Name))
            .ForMember(x => x.Description, x => x.MapFrom(x => x.Translations.First().Description));
    }
}