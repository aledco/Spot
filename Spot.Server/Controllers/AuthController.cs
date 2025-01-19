using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spot.Business.Contracts.Spotify;

namespace Spot.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseController
    {
        private readonly ISpotifyApiService _spotifyApiService;
        private readonly IConfiguration _configuration;

        public AuthController(ISpotifyApiService spotifyApiService, IConfiguration configuration) 
        {
            this._configuration = configuration;
            this._spotifyApiService = spotifyApiService;
        }

        [HttpGet]
        [Route("Spotify/{authCode}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSpotifyAccessTokenAsync([FromRoute] string authCode)
        {
            return await this._spotifyApiService.GetAccessTokenAsync(authCode);
        }
    }
}
