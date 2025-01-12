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
        private readonly ISongTagCategoryRepository _songTagCategoryRepository;
        private readonly ISongSongTagMapRepository _songSongTagMapRepository;
        private readonly IUserService _userService;
        private readonly ISpotifyApiService _spotifyApiService;

        public SongService(
            IMapper mapper,
            ISongRepository songRepository,
            ISongTagRepository songTagRepository,
            ISongTagCategoryRepository songTagCategoryRepository,
            ISongSongTagMapRepository songSongTagMapRepository,
            IUserService userService,
            ISpotifyApiService spotifyApiService)
            : base(mapper)
        {
            this._songRepository = songRepository;
            this._songTagRepository = songTagRepository;
            this._songTagCategoryRepository = songTagCategoryRepository;
            this._songSongTagMapRepository = songSongTagMapRepository;
            this._userService = userService;
            this._spotifyApiService = spotifyApiService;
        }

        public async Task<IList<SongModel>> GetAllAsync(string spotifyAccessToken)
        {
            var user = await this._userService.GetUserAsync(spotifyAccessToken);
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
            var user = await this._userService.GetUserAsync(spotifyAccessToken);
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

        public async Task<SongModel> SaveAsync(string spotifyAccessToken, SongModel model)
        {
            var entity = this._mapper.Map<Song>(model);
            var savedEntity = await this._songRepository.SaveAsync(entity);
            return this._mapper.Map<SongModel>(savedEntity);
        }

        public async Task<IList<SongTagModel>> GetAllTagsAsync(string spotifyAccessToken)
        {
            var user = await this._userService.GetUserAsync(spotifyAccessToken);
            if (user == null)
            {
                return null;
            }

            var entities = await this._songTagRepository.GetAllByUserIdAsync(user.Id);
            var models = this._mapper.Map<List<SongTagModel>>(entities);
            return models;
        }

        public async Task<IList<SongModel>> SyncAsync(string spotifyAccessToken) // TODO use claims
        {
            // TODO remove songs not in playlists
            var user = await this._userService.GetUserAsync(spotifyAccessToken);
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
                        Name = playlist.Name
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
