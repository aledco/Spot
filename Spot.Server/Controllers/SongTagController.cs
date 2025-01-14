using Microsoft.AspNetCore.Mvc;
using Spot.Business.Contracts;
using Spot.Business.Models;
using Spot.Business.Models.Result;

namespace Spot.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SongTagController : BaseController
    {

        private readonly ISongTagService _songTagService;

        public SongTagController(ISongTagService songService) 
        {
            this._songTagService = songService;
        }

        [HttpGet]
        [Route("")]
        public async Task<OperationResult<IList<SongTagModel>>> GetAllAsync()
        {
            if (string.IsNullOrEmpty(this.SpotifyAccessToken))
            {
                return OperationResult<IList<SongTagModel>>.Fatal();
            }

            return await this._songTagService.GetAllAsync(this.SpotifyAccessToken);
        }

        [HttpGet]
        [Route("{songTagId:int}")]
        public async Task<OperationResult<SongTagModel>> GetAsync([FromRoute] int songTagId)
        {
            if (string.IsNullOrEmpty(this.SpotifyAccessToken))
            {
                return OperationResult<SongTagModel>.Fatal();
            }

            return await this._songTagService.GetAsync(this.SpotifyAccessToken, songTagId);
        }

        [HttpPost]
        [Route("")]
        public async Task<OperationResult<SongTagModel>> SaveAsync([FromBody] SongTagModel model)
        {
            if (string.IsNullOrEmpty(this.SpotifyAccessToken))
            {
                return OperationResult<SongTagModel>.Fatal();
            }

            return await this._songTagService.SaveAsync(this.SpotifyAccessToken, model);
        }
    }
}
