using Microsoft.AspNetCore.Mvc;
using Spot.Business.Contracts;
using Spot.Business.Models;
using Spot.Server.Authorization;

namespace Spot.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [SpotifyAccessCodeAuthorization]
    public class SongTagController : BaseController
    {

        private readonly ISongTagService _songTagService;

        public SongTagController(ISongTagService songService) 
        {
            this._songTagService = songService;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetAllAsync()
        {
            return await this._songTagService.GetAllAsync();
        }

        [HttpGet]
        [Route("{songTagId:int}")]
        public async Task<IActionResult> GetAsync([FromRoute] int songTagId)
        {
            return await this._songTagService.GetAsync(songTagId);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> SaveAsync([FromBody] SongTagModel model)
        {
            return await this._songTagService.SaveAsync( model);
        }

        [HttpDelete]
        [Route("{songTagId:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int songTagId)
        {
            return await this._songTagService.DeleteAsync(songTagId);
        }
    }
}
