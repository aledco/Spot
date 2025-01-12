using AutoMapper;
using Spot.Business.Models;
using Spot.Data.Entities;

namespace Spot.Business.Maps
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            this.CreateMap<User, UserModel>()
                .ReverseMap();

            this.CreateMap<Song, SongModel>()
                .ForMember(dst => dst.Tags, opt => opt.MapFrom(src => src.SongTags))
                .ReverseMap();

            this.CreateMap<SongTag, SongTagModel>()
                .ReverseMap();

            this.CreateMap<SongTagCategory, SongTagCategoryModel>()
                .ReverseMap();
        }  
    }
}
