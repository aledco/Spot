using Microsoft.AspNetCore.Mvc;
using Spot.Business.Contracts.Spotify;
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
        public async Task<SpotifyPlaylistPage> GetAllAsync()
        {
            if (string.IsNullOrEmpty(this.SpotifyAccessToken))
            {
                return null; // TODO implement system for returning errors
            }

            return await this._spotifyPlaylistService.GetAllAsync(this.SpotifyAccessToken);
        }
    }
}
