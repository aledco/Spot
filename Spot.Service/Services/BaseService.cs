using AutoMapper;
using Microsoft.AspNetCore.Http;

namespace Spot.Business.Services
{
    public abstract class BaseService
    {
        protected readonly HttpContext _httpContext;
        protected readonly IMapper _mapper;

        protected string SpotifyAccessToken { get => this._httpContext.Request.Headers["Spotify-Access-Token"]; }

        public BaseService(IHttpContextAccessor contextAccessor, IMapper mapper)
        {
            this._httpContext = contextAccessor.HttpContext;
            this._mapper = mapper;
        }
    }
}
