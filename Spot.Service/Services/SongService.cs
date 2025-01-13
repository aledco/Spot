using AutoMapper;
using Spot.Business.Contracts;
using Spot.Business.Contracts.Spotify;
using Spot.Business.Models;
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

        public async Task<IList<SongModel>> GetAllAsync(string spotifyAccessToken)
        {
            var user = await this._userService.GetAsync(spotifyAccessToken);
            if (user == null)
            {
                return null;
            }

            var entities = await this._songRepository.GetAllByUserIdAsync(user.Id);
            var models = this._mapper.Map<List<SongModel>>(entities);
            return models;
        }

        public async Task<SongModel> GetAsync(string spotifyAccessToken, int songId)
        {
            var user = await this._userService.GetAsync(spotifyAccessToken);
            if (user == null)
            {
                return null;
            }

            var entity = await this._songRepository.GetAsync(songId);
            if (entity == null)
            {
                return null; // TODO error
            }

            if (entity.UserId != user.Id)
            {
                return null; // TODO error
            }

            var model = this._mapper.Map<SongModel>(entity);
            return model;
        }

        public async Task<SongModel> SaveAsync(string spotifyAccessToken, SongModel model) // TODO rename to update
        {
            // TODO sync spotify playlists for tags

            var existingMaps = await this._songSongTagMapRepository.GetAllBySongIdAsync(model.Id);

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

            var existingEntity = await this._songRepository.GetAsync(model.Id);
            if (existingEntity == null)
            {
                return null; // TODO need to support adding a song too, probably seperate endpont since we need to search spotify
            }

            existingEntity.Name = model.Name;
            existingEntity.Artist = model.Artist;
            var savedEntity = await this._songRepository.SaveAsync(existingEntity);
            return this._mapper.Map<SongModel>(savedEntity);
        }

        public async Task<IList<SongModel>> SyncSongsFromPlaylistsAsync(string spotifyAccessToken) // TODO use claims
        {
            // TODO remove songs not in playlists
            var user = await this._userService.GetAsync(spotifyAccessToken);
            if (user == null)
            {
                return null;
            }

            var songs = new List<SongModel>();
            var playlistTracks = await this._spotifyApiService.GetAllTracksFromPlaylistsAsync(spotifyAccessToken);
            foreach (var (playlist, tracks) in playlistTracks)
            {
                var tag = await this._songTagRepository.GetByPlaylistNameAsync(user.Id, playlist.Name);
                if (tag == null)
                {
                    tag = await this._songTagRepository.SaveAsync(new SongTag
                    {
                        UserId = user.Id,
                        Name = playlist.Name,
                        SpotifyId = playlist.Id,
                    });
                }

                foreach (var track in tracks)
                {
                    var song = await this._songRepository.GetByUserIdAndSpotifyIdAsync(user.Id, track.Id);
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

            return songs;
        }
    }
}
