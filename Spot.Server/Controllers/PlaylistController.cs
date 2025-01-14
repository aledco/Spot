using Microsoft.AspNetCore.Mvc;
using Spot.Business.Contracts.Spotify;
using Spot.Business.Models.Result;
using Spot.Business.Models.Spotify;

namespace Spot.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlaylistController : BaseController
    {
        private readonly ISpotifyApiService _spotifyPlaylistService;

        public PlaylistController(ISpotifyApiService spotifyPlaylistService) 
        {
            this._spotifyPlaylistService = spotifyPlaylistService;
        }

        [HttpGet]
        [Route("")]
        public async Task<OperationResult<IList<SimplifiedSpotifyPlaylist>>> GetAllAsync()
        {
            if (string.IsNullOrEmpty(this.SpotifyAccessToken))
            {
                return OperationResult<IList<SimplifiedSpotifyPlaylist>>.Failed();
            }

            return await this._spotifyPlaylistService.GetAllPlaylistsAsync(this.SpotifyAccessToken);
        }
    }
}
