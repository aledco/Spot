using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Spot.Business.Contracts.Spotify;
using Spot.Business.Contracts;
using Spot.Business.Services.Spotify;
using Spot.Business.Services;
using Spot.Data;
using Spot.Data.Contracts;
using Spot.Data.Repositories;
using Spot.Business.Maps;

namespace Spot.Server
{
    public static class WebApplicationBuilderExtensions
    {
        public static void ConfigureDependencyInjection(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<ISpotifyApiService, SpotifyApiService>();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddTransient<ISongService, SongService>();
            builder.Services.AddTransient<ISongTagService, SongTagService>();
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<ISongRepository, SongRepository>();
            builder.Services.AddTransient<ISongTagRepository, SongTagRepository>();
            builder.Services.AddTransient<ISongTagCategoryRepository, SongTagCategoryRepository>();
            builder.Services.AddTransient<ISongSongTagMapRepository, SongSongTagMapRepository>();
        }

        public static void ConfigureAutoMapper(this WebApplicationBuilder builder)
        {
            var mapperConfig = new MapperConfiguration(mc => mc.AddProfile(new AutoMapperProfile()));
            var mapper = mapperConfig.CreateMapper();
            builder.Services.AddSingleton(mapper);
        }

        public static void ConfigureEntityFramework(this WebApplicationBuilder builder)
        {
            var connectionString = builder.Configuration.GetConnectionString("Spot");
            builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connectionString));
        }
    }
}
