using AutoMapper;
using Spot.Business.Contracts;
using Spot.Business.Contracts.Spotify;
using Spot.Business.Models;
using Spot.Business.Models.Result;
using Spot.Business.Resources;
using Spot.Data.Contracts;
using Spot.Data.Entities;

namespace Spot.Business.Services
{
    public class SongService : BaseService, ISongService
    {
        private readonly ISongRepository _songRepository;
        private readonly ISongTagRepository _songTagRepository;
        private readonly ISongSongTagMapRepository _songSongTagMapRepository;
        private readonly IUserService _userService;
        private readonly ISpotifyApiService _spotifyApiService;

        public SongService(
            IMapper mapper,
            ISongRepository songRepository,
            ISongTagRepository songTagRepository,
            ISongSongTagMapRepository songSongTagMapRepository,
            IUserService userService,
            ISpotifyApiService spotifyApiService)
            : base(mapper)
        {
            this._songRepository = songRepository;
            this._songTagRepository = songTagRepository;
            this._songSongTagMapRepository = songSongTagMapRepository;
            this._userService = userService;
            this._spotifyApiService = spotifyApiService;
        }

        public async Task<OperationResult<IList<SongModel>>> GetAllAsync(string spotifyAccessToken)
        {
            var userResult = await this._userService.GetAsync(spotifyAccessToken);
            if (!userResult.IsValid)
            {
                return userResult.ErrorsAs<IList<SongModel>>();
            }
            
            var user = userResult.Result;
            var entities = await this._songRepository.GetAllByUserIdAsync(user.Id.Value);
            var models = this._mapper.Map<List<SongModel>>(entities);
            return OperationResult<IList<SongModel>>.Success(models);
        }

        public async Task<OperationResult<SongModel>> GetAsync(string spotifyAccessToken, int songId)
        {
            var userResult = await this._userService.GetAsync(spotifyAccessToken);
            if (!userResult.IsValid)
            {
                return userResult.ErrorsAs<SongModel>();
            }

            var user = userResult.Result;

            var entity = await this._songRepository.GetAsync(songId);
            if (entity == null)
            {
                return OperationResult<SongModel>.Failed();
            }

            if (entity.UserId != user.Id)
            {
                return OperationResult<SongModel>.Failed();
            }

            var model = this._mapper.Map<SongModel>(entity);
            return OperationResult<SongModel>.Success(model);
        }

        public async Task<OperationResult<SongModel>> SaveAsync(string spotifyAccessToken, SongModel model) // TODO rename to update
        {
            var existingMaps = await this._songSongTagMapRepository.GetAllBySongIdAsync(model.Id.Value);

            var mapsToAdd = model.TagIds
                .Where(tagId => !existingMaps.Any(map => map.SongTagId == tagId))
                .Select(tagId => new SongSongTagMap()
                { 
                    SongId = model.Id,
                    SongTagId = tagId,   
                })
                .ToList();

            var mapsToDelete = existingMaps
                .Where(map => !model.TagIds.Any(tagId => tagId == map.SongTagId))
                .ToList();

            foreach (var map in mapsToAdd)
            {
                var tagEntity = await this._songTagRepository.GetAsync(map.SongTagId.Value);
                var tagModel = this._mapper.Map<SongTagModel>(tagEntity);
                await this._spotifyApiService.AddSongToPlaylistForSongTagAsync(spotifyAccessToken, tagModel, model);
            }

            foreach (var map in mapsToDelete)
            {
                var tagEntity = await this._songTagRepository.GetAsync(map.SongTagId.Value);
                var tagModel = this._mapper.Map<SongTagModel>(tagEntity);
                await this._spotifyApiService.RemoveSongFromPlaylistForSongTagAsync(spotifyAccessToken, tagModel, model);
            }

            await this._songSongTagMapRepository.SaveRangeAsync(mapsToAdd);
            await this._songSongTagMapRepository.DeleteRangeAsync(mapsToDelete);

            var existingEntity = await this._songRepository.GetAsync(model.Id.Value);
            if (existingEntity == null)
            {
                return OperationResult<SongModel>.Failed(); // TODO need to support adding a song too, probably seperate endpont since we need to search spotify
            }

            existingEntity.Name = model.Name;
            existingEntity.Artist = model.Artist;
            var savedEntity = await this._songRepository.SaveAsync(existingEntity);
            return OperationResult<SongModel>.Success(this._mapper.Map<SongModel>(savedEntity));
        }

        public async Task<OperationResult<IList<SongModel>>> SyncSongsFromPlaylistsAsync(string spotifyAccessToken) // TODO use claims
        {
            return OperationResult<IList<SongModel>>.Failed(ErrorMessages.UserNotFound);

            // TODO remove songs not in playlists
            var userResult = await this._userService.GetAsync(spotifyAccessToken);
            if (!userResult.IsValid)
            {
                return userResult.ErrorsAs<IList<SongModel>>();
            }

            var user = userResult.Result;

            var songs = new List<SongModel>();
            var playlistTracksResult = await this._spotifyApiService.GetAllTracksFromPlaylistsAsync(spotifyAccessToken);
            if (!playlistTracksResult.IsValid)
            {
                return playlistTracksResult.ErrorsAs<IList<SongModel>> ();
            }

            var playlistTracks = playlistTracksResult.Result;
            foreach (var (playlist, tracks) in playlistTracks)
            {
                var tag = await this._songTagRepository.GetByPlaylistNameAsync(user.Id.Value, playlist.Name);
                if (tag == null)
                {
                    tag = await this._songTagRepository.SaveAsync(new SongTag
                    {
                        UserId = user.Id,
                        Name = playlist.Name,
                        SpotifyId = playlist.Id,
                        IsPublic = playlist.Public,
                        Description = playlist.Description,
                    });
                }

                foreach (var track in tracks)
                {
                    var song = await this._songRepository.GetByUserIdAndSpotifyIdAsync(user.Id.Value, track.Id);
                    if (song == null)
                    {
                        song = await this._songRepository.SaveAsync(new Song()
                        {
                            UserId = user.Id,
                            SpotifyId = track.Id,
                            Name = track.Name,
                            Artist = string.Join(" | ", track.Artists.Select(a => a.Name))
                        });
                    }
                    
                    if (!song.SongTags.Any(t => t.Id == tag.Id))
                    {
                        var songSongTagMap = await this._songSongTagMapRepository.SaveAsync(new SongSongTagMap()
                        {
                            SongId = song.Id,
                            SongTagId = tag.Id
                        });

                        song = songSongTagMap.Song;
                    }

                    songs.Add(this._mapper.Map<SongModel>(song));
                }
            }

            return OperationResult<IList<SongModel>>.Success(songs);
        }
    }
}
