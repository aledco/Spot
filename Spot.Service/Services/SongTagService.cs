using AutoMapper;
using Microsoft.AspNetCore.Http;
using Spot.Business.Contracts;
using Spot.Business.Contracts.Spotify;
using Spot.Business.Models;
using Spot.Business.Models.Result;
using Spot.Business.Resources;
using Spot.Data.Contracts;
using Spot.Data.Entities;

namespace Spot.Business.Services
{
    public class SongTagService : BaseService, ISongTagService
    {
        private readonly ISongTagRepository _songTagRepository;
        private readonly ISongTagCategoryRepository _songTagCategoryRepository;
        private readonly ISongSongTagMapRepository _songSongTagMapRepository;
        private readonly IUserService _userService;
        private readonly ISpotifyApiService _spotifyApiService;

        public SongTagService(
            IHttpContextAccessor contextAccessor,
            IMapper mapper,
            ISongTagRepository songTagRepository,
            ISongTagCategoryRepository songTagCategoryRepository,
            ISongSongTagMapRepository songSongTagMapRepository,
            IUserService userService,
            ISpotifyApiService spotifyApiService)
            : base(contextAccessor, mapper)
        {
            this._songTagRepository = songTagRepository;
            this._songTagCategoryRepository = songTagCategoryRepository;
            this._songSongTagMapRepository = songSongTagMapRepository;
            this._userService = userService;
            this._spotifyApiService = spotifyApiService;
        }

        public async Task<OperationResult<IList<SongTagModel>>> GetAllAsync()
        {
            var userResult = await this._userService.GetCurrentUserAsync();
            if (!userResult.IsValid)
            {
                return userResult.ErrorsAs<IList<SongTagModel>>();
            }

            var user = userResult.Result;
            var entities = await this._songTagRepository.GetAllByUserIdAsync(user.Id.Value);
            var models = this._mapper.Map<List<SongTagModel>>(entities);
            return OperationResult<IList<SongTagModel>>.Success(models);
        }

        public async Task<OperationResult<SongTagModel>> GetAsync(int songTagId)
        {
            var userResult = await this._userService.GetCurrentUserAsync();
            if (!userResult.IsValid)
            {
                return userResult.ErrorsAs<SongTagModel>();
            }

            var user = userResult.Result;
            var entity = await this._songTagRepository.GetAsync(songTagId);
            if (entity == null)
            {
                return OperationResult<SongTagModel>.Failed();
            }

            if (entity.UserId != user.Id)
            {
                return OperationResult<SongTagModel>.Failed();
            }

            var model = this._mapper.Map<SongTagModel>(entity);
            return OperationResult<SongTagModel>.Success(model);
        }

        public async Task<OperationResult<SongTagModel>> SaveAsync(SongTagModel model)
        {
            if (model.Id == null)
            {
                var playlistResult = await this._spotifyApiService.CreatePlaylistFromSongTagAsync(this.SpotifyAccessToken, model);
                if (!playlistResult.IsValid)
                {
                    return playlistResult.ErrorsAs<SongTagModel>();
                }

                model.SpotifyId = playlistResult.Result.Id;
            }
            else
            {
                var result = await this._spotifyApiService.UpdatePlaylistFromSongTagAsync(this.SpotifyAccessToken, model);
                if (!result.IsValid)
                {
                    return result.ErrorsAs<SongTagModel>();
                }
            }

            var userResult = await this._userService.GetCurrentUserAsync();
            if (!userResult.IsValid)
            {
                return userResult.ErrorsAs<SongTagModel>();
            }

            var user = userResult.Result;
            var entity = this._mapper.Map<SongTag>(model);
            entity.UserId = user.Id;
            var savedEntity = await this._songTagRepository.SaveAsync(entity);
            return OperationResult<SongTagModel>.Success(this._mapper.Map<SongTagModel>(savedEntity));
        }

        public async Task<OperationResult> DeleteAsync(int songTagId)
        {
            var entity = await this._songTagRepository.GetAsync(songTagId);
            if (entity == null)
            {
                return OperationResult.Failed();
            }

            await this._spotifyApiService.DeletePlaylistAsync(this.SpotifyAccessToken, entity.SpotifyId);

            await this._songSongTagMapRepository.DeleteRangeAsync(entity.SongSongTagMaps);
            await this._songTagRepository.DeleteAsync(entity);
            return OperationResult.Success();
        }
    }
}
