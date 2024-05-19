using AutoMapper;
using Guide.Domain.Entities;
using Guide.Shared.Common.Interfaces;

namespace Guide.Shared.Common.Dtos;

public class AttractionImageDto : IMapFrom<AttractionImage>
{
    public int Id { get; set; }
    public string Url { get; set; } = null!;
    public bool IsMain { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<AttractionImage, AttractionImageDto>();
    }
}