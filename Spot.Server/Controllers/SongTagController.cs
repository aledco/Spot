using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Spot.Business.Contracts;
using Spot.Business.Models;

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
        public async Task<IList<SongTagModel>> GetAllAsync()
        {
            if (string.IsNullOrEmpty(this.SpotifyAccessToken))
            {
                return null; // TODO implement system for returning errors
            }

            return await this._songTagService.GetAllAsync(this.SpotifyAccessToken);
        }

        [HttpGet]
        [Route("{songTagId:int}")]
        public async Task<SongTagModel> GetAsync([FromRoute] int songTagId)
        {
            if (string.IsNullOrEmpty(this.SpotifyAccessToken))
            {
                return null; // TODO implement system for returning errors
            }

            return await this._songTagService.GetAsync(this.SpotifyAccessToken, songTagId);
        }

        [HttpPost]
        [Route("")]
        public async Task<SongTagModel> SaveAsync([FromBody] SongTagModel model)
        {
            if (string.IsNullOrEmpty(this.SpotifyAccessToken))
            {
                return null; // TODO implement system for returning errors
            }

            return await this._songTagService.SaveAsync(this.SpotifyAccessToken, model);
        }
    }
}
