using Microsoft.AspNetCore.Mvc;
using Spot.Business.Contracts;
using Spot.Business.Models;

namespace Spot.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongController : BaseController
    {

        private readonly ISongService _songService;

        public SongController(ISongService songService) 
        {
            this._songService = songService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IList<SongModel>> GetAllAsync()
        {
            if (string.IsNullOrEmpty(this.SpotifyAccessToken))
            {
                return null; // TODO implement system for returning errors
            }

            return await this._songService.GetAllAsync(this.SpotifyAccessToken);
        }

        [HttpGet]
        [Route("{songId:int}")]
        public async Task<SongModel> GetAsync([FromRoute] int songId)
        {
            if (string.IsNullOrEmpty(this.SpotifyAccessToken))
            {
                return null; // TODO implement system for returning errors
            }

            return await this._songService.GetAsync(this.SpotifyAccessToken, songId);
        }

        [HttpPost]
        [Route("")]
        public async Task<SongModel> SaveAsync([FromBody] SongModel model)
        {
            if (string.IsNullOrEmpty(this.SpotifyAccessToken))
            {
                return null; // TODO implement system for returning errors
            }

            return await this._songService.SaveAsync(this.SpotifyAccessToken, model);
        }

        [HttpPost]
        [Route("Sync")]
        public async Task<IList<SongModel>> SyncSongsFromPlaylistsAsync()
        {
            if (string.IsNullOrEmpty(this.SpotifyAccessToken))
            {
                return null; // TODO implement system for returning errors
            }

            return await this._songService.SyncSongsFromPlaylistsAsync(this.SpotifyAccessToken);
        }
    }
}
