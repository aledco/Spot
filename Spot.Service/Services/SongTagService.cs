using AutoMapper;
using Spot.Business.Contracts;
using Spot.Business.Contracts.Spotify;
using Spot.Business.Models;
using Spot.Data.Contracts;
using Spot.Data.Entities;

namespace Spot.Business.Services
{
    public class SongTagService : BaseService, ISongTagService
    {
        private readonly ISongTagRepository _songTagRepository;
        private readonly ISongTagCategoryRepository _songTagCategoryRepository;
        private readonly IUserService _userService;
        private readonly ISpotifyApiService _spotifyApiService;

        public SongTagService(
            IMapper mapper,
            ISongTagRepository songTagRepository,
            ISongTagCategoryRepository songTagCategoryRepository,
            IUserService userService,
            ISpotifyApiService spotifyApiService)
            : base(mapper)
        {
            this._songTagRepository = songTagRepository;
            this._songTagCategoryRepository = songTagCategoryRepository;
            this._userService = userService;
            this._spotifyApiService = spotifyApiService;
        }

        public async Task<IList<SongTagModel>> GetAllAsync(string spotifyAccessToken)
        {
            var user = await this._userService.GetAsync(spotifyAccessToken);
            if (user == null)
            {
                return null;
            }

            var entities = await this._songTagRepository.GetAllByUserIdAsync(user.Id);
            var models = this._mapper.Map<List<SongTagModel>>(entities);
            return models;
        }

        public async Task<SongTagModel> GetAsync(string spotifyAccessToken, int songTagId)
        {
            var user = await this._userService.GetAsync(spotifyAccessToken);
            if (user == null)
            {
                return null;
            }

            var entity = await this._songTagRepository.GetAsync(songTagId);
            if (entity == null)
            {
                return null; // TODO error
            }

            if (entity.UserId != user.Id)
            {
                return null; // TODO error
            }

            var model = this._mapper.Map<SongTagModel>(entity);
            return model;
        }

        public async Task<SongTagModel> SaveAsync(string spotifyAccessToken, SongTagModel model)
        {
            // TODO sync spotify playlist for tag if tag exists (in case name changed), only need to make a new playlist when a song is given a tag
            var user = await this._userService.GetAsync(spotifyAccessToken);
            var entity = this._mapper.Map<SongTag>(model);
            entity.UserId = user.Id;
            var savedEntity = await this._songTagRepository.SaveAsync(entity);
            return this._mapper.Map<SongTagModel>(savedEntity);
        }
    }
}
