using Microsoft.AspNetCore.Mvc;
using Spot.Business.Contracts;
using Spot.Business.Models;
using Spot.Server.Authorization;

namespace Spot.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SpotifyAccessCodeAuthorization]
    public class SongController : BaseController
    {

        private readonly ISongService _songService;

        public SongController(ISongService songService) 
        {
            this._songService = songService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllAsync()
        {
            return await this._songService.GetAllAsync();
        }

        [HttpGet]
        [Route("{songId:int}")]
        public async Task<IActionResult> GetAsync([FromRoute] int songId)
        {
            return await this._songService.GetAsync(songId);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> SaveAsync([FromBody] SongModel model)
        {
            return await this._songService.SaveAsync(model);
        }

        [HttpDelete]
        [Route("{songId:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int songId)
        {

            return await this._songService.DeleteAsync(songId);
        }

        [HttpPost]
        [Route("Search")]
        public async Task<IActionResult> SearchAsync([FromBody] SongSearchCriteriaModel searchCriteria)
        {
            return await this._songService.SearchAsync(searchCriteria);
        }

        [HttpPost]
        [Route("Sync")]
        public async Task<IActionResult> SyncSongsFromPlaylistsAsync()
        {
            return await this._songService.SyncSongsFromPlaylistsAsync();
        }
    }
}
