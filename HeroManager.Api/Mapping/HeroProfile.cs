using AutoMapper;
using HeroManager.Api.Domain.Entities;
using HeroManager.Api.Models.Requests;
using HeroManager.Api.Models.Responses;

namespace HeroManager.Api.Mapping
{
    public class HeroProfile : Profile
    {
        public HeroProfile()
        {
            CreateMap<HeroCreateRequest, Hero>();
            CreateMap<Hero, HeroResponse>()
                .ForMember(dest => dest.Superpoderes, opt =>
                    opt.MapFrom(src => src.HeroSuperpowers.Select(hs => hs.Superpoder)));

            CreateMap<Superpower, SuperpowerResponse>();
        }
    }
}
