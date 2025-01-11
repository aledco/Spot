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
                .ReverseMap();

            this.CreateMap<SongTag, SongTagModel>()
                .ReverseMap();

            this.CreateMap<SongTagCategory, SongTagCategoryModel>()
                .ReverseMap();
        }  
    }
}
