using AutoMapper;
using Guide.Domain.Entities;
using Guide.Shared.Common.Interfaces;

namespace Guide.Shared.Common.Dtos;

public class ReviewDto : IMapFrom<Review>
{
    public int Id { get; set; }
    public string UserId { get; set; } = null!;
    public string UserName { get; set; } = null!;
    public string Comment { get; set; } = null!;
    public byte Rating { get; set; }
    public DateTime Submitted { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Review, ReviewDto>()
            .ForMember(x => x.UserId, x => x.MapFrom(y => y.UserId))
            .ForMember(x => x.UserName, x => x.MapFrom(y => y.User.UserName))
            .ForMember(x => x.Submitted, x => x.MapFrom(y => y.Modified));
    }
}