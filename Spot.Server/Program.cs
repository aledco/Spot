using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Spot.Business.Contracts;
using Spot.Business.Contracts.Spotify;
using Spot.Business.Maps;
using Spot.Business.Services;
using Spot.Business.Services.Spotify;
using Spot.Data;
using Spot.Data.Contracts;
using Spot.Data.Repositories;

void ConfigureDependencyInjection(IServiceCollection services)
{
    services.AddTransient<ISpotifyApiService, SpotifyApiService>();
    services.AddTransient<IUserService, UserService>();
    services.AddTransient<ISongService, SongService>();
    services.AddTransient<ISongTagService, SongTagService>();
    services.AddTransient<IUserRepository, UserRepository>();
    services.AddTransient<ISongRepository, SongRepository>();
    services.AddTransient<ISongTagRepository, SongTagRepository>();
    services.AddTransient<ISongTagCategoryRepository, SongTagCategoryRepository>();
    services.AddTransient<ISongSongTagMapRepository, SongSongTagMapRepository>();
}

void ConfigureEntityFramework(IServiceCollection services)
{
    // TODO use config
    services.AddDbContext<ApplicationContext>(options => options.UseSqlServer("Data Source=.;Initial Catalog=Spot;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True;"));
}

void ConfigureAutoMapper(IServiceCollection services)
{
    var mapperConfig = new MapperConfiguration(mc =>
    {
        mc.AddProfile(new AutoMapperProfile());
    });

    var mapper = mapperConfig.CreateMapper();
    services.AddSingleton(mapper);
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
    builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowAnyMethod()
            .WithExposedHeaders("content-disposition")
            .AllowAnyHeader()
            .SetPreflightMaxAge(TimeSpan.FromSeconds(3600));
    });
});

ConfigureDependencyInjection(builder.Services);
ConfigureEntityFramework(builder.Services);
ConfigureAutoMapper(builder.Services);

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors("AllowAll");

app.Use(async (context, next) =>
{
    context.Response.Headers.Append("Access-Control-Allow-Origin", "*");
    await next.Invoke();
});

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("/index.html");

app.Run();
