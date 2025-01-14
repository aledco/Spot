using Microsoft.AspNetCore.Mvc;
using Spot.Business.Contracts;
using Spot.Business.Models;
using Spot.Business.Models.Result;

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
        public async Task<OperationResult<IList<SongModel>>> GetAllAsync()
        {
            if (string.IsNullOrEmpty(this.SpotifyAccessToken))
            {
                return OperationResult<IList<SongModel>>.Fatal();
            }

            return await this._songService.GetAllAsync(this.SpotifyAccessToken);
        }

        [HttpGet]
        [Route("{songId:int}")]
        public async Task<OperationResult<SongModel>> GetAsync([FromRoute] int songId)
        {
            if (string.IsNullOrEmpty(this.SpotifyAccessToken))
            {
                return OperationResult<SongModel>.Fatal();
            }

            return await this._songService.GetAsync(this.SpotifyAccessToken, songId);
        }

        [HttpPost]
        [Route("")]
        public async Task<OperationResult<SongModel>> SaveAsync([FromBody] SongModel model)
        {
            if (string.IsNullOrEmpty(this.SpotifyAccessToken))
            {
                return OperationResult<SongModel>.Fatal();
            }

            return await this._songService.SaveAsync(this.SpotifyAccessToken, model);
        }

        [HttpDelete]
        [Route("{songId:int}")]
        public async Task<OperationResult> DeleteAsync([FromRoute] int songId)
        {
            if (string.IsNullOrEmpty(this.SpotifyAccessToken))
            {
                return OperationResult.Fatal();
            }

            return await this._songService.DeleteAsync(this.SpotifyAccessToken, songId);
        }

        [HttpPost]
        [Route("Search")]
        public async Task<OperationResult<IList<SongModel>>> SearchSongsAsync([FromBody] SongSearchCriteriaModel searchCriteria)
        {
            if (string.IsNullOrEmpty(this.SpotifyAccessToken))
            {
                return OperationResult<IList<SongModel>>.Fatal();
            }

            return await this._songService.SearchSongsAsync(this.SpotifyAccessToken, searchCriteria);
        }

        [HttpPost]
        [Route("Sync")]
        public async Task<OperationResult<IList<SongModel>>> SyncSongsFromPlaylistsAsync()
        {
            if (string.IsNullOrEmpty(this.SpotifyAccessToken))
            {
                return OperationResult<IList<SongModel>>.Fatal();
            }

            return await this._songService.SyncSongsFromPlaylistsAsync(this.SpotifyAccessToken);
        }
    }
}
