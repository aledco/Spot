using Spot.Business.Models;
using Spot.Business.Models.Spotify;

namespace Spot.Business.Contracts.Spotify
{
    public interface ISpotifyApiService
    {
        Task<SpotifyUser> GetSpotifyUserAsync(string spotifyAccessToken);
        Task<SpotifySearchResult> SearchAsync(string spotifyAccessToken, SpotifySearchCriteria searchCriteria);
        Task<IList<SpotifyTrack>> GetAllTracksForPlaylistAsync(string spotifyAccessToken, string playlistId);
        Task<IList<(SimplifiedSpotifyPlaylist Playlist, IList<SpotifyTrack> Tracks)>> GetAllTracksFromPlaylistsAsync(string spotifyAccessToken);
        Task<SpotifyPlaylist?> GetPlaylistAsync(string spotifyAccessToken, string playlistId);
        Task<SpotifyPlaylistPage> GetAllPlaylistsAsync(string spotifyAccessToken);
        Task<SpotifyPlaylist> CreatePlaylistFromSongTagAsync(string spotifyAccessToken, SongTagModel songTag);
        Task AddTracksToPlaylistAsync(string spotifyAccessToken, string playlistId, IEnumerable<string> trackSpotifyIds);
        Task RemoveTracksFromPlaylistAsync(string spotifyAccessToken, string playlistId, IEnumerable<string> trackSpotifyIds);
        Task AddSongToPlaylistForSongTagAsync(string spotifyAccessToken, SongTagModel songTag, SongModel song);
        Task RemoveSongFromPlaylistForSongTagAsync(string spotifyAccessToken, SongTagModel songTag, SongModel song);
        Task SyncPlaylistForSongTagAsync(string spotifyAccessToken, SongTagModel songTag, IList<SongModel> songs);
    }
}
