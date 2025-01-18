using Microsoft.AspNetCore.Mvc;
using Spot.Business.Contracts.Spotify;
using Spot.Server.Authorization;

namespace Spot.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SpotifyAccessCodeAuthorization]
    public class PlaylistController : BaseController
    {
        private readonly ISpotifyApiService _spotifyPlaylistService;

        public PlaylistController(ISpotifyApiService spotifyPlaylistService) 
        {
            this._spotifyPlaylistService = spotifyPlaylistService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllAsync()
        {
            var spotifyAccessToken = this.Request.Headers["Spotify-Access-Token"];
            return await this._spotifyPlaylistService.GetAllPlaylistsAsync(spotifyAccessToken);
        }

        [HttpPut]
        [Route("{playlistId}/Shuffle")]
        public async Task<IActionResult> ShufflePlaylisyAsync([FromRoute] string playlistId)
        {
            var spotifyAccessToken = this.Request.Headers["Spotify-Access-Token"];
            return await this._spotifyPlaylistService.ShufflePlaylistAsync(spotifyAccessToken, playlistId);
        }
    }
}
