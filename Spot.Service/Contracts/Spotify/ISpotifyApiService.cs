using Spot.Business.Models;
using Spot.Business.Models.Result;
using Spot.Business.Models.Spotify;
using System.Threading.Tasks;

namespace Spot.Business.Contracts.Spotify
{
    public interface ISpotifyApiService
    {
        Task<OperationResult<SpotifyUser>> GetSpotifyUserAsync(string spotifyAccessToken);
        Task<OperationResult<SpotifySearchResult>> SearchAsync(string spotifyAccessToken, SpotifySearchCriteria searchCriteria);
        Task<OperationResult<IList<SpotifyTrack>>> GetAllTracksForPlaylistAsync(string spotifyAccessToken, string playlistId);
        Task<OperationResult<IList<(SimplifiedSpotifyPlaylist Playlist, IList<SpotifyTrack> Tracks)>>> GetAllTracksFromPlaylistsAsync(string spotifyAccessToken);
        Task<OperationResult<SpotifyPlaylist?>> GetPlaylistAsync(string spotifyAccessToken, string playlistId);
        Task<OperationResult> DeletePlaylistAsync(string spotifyAccessToken, string playlistId);
        Task<OperationResult<IList<SimplifiedSpotifyPlaylist>>> GetAllPlaylistsAsync(string spotifyAccessToken);
        Task<OperationResult<SpotifyPlaylist>> CreatePlaylistFromSongTagAsync(string spotifyAccessToken, SongTagModel songTag);
        Task<OperationResult> UpdatePlaylistFromSongTagAsync(string spotifyAccessToken, SongTagModel songTag);
        Task<OperationResult> AddTracksToPlaylistAsync(string spotifyAccessToken, string playlistId, IEnumerable<string> trackSpotifyIds);
        Task<OperationResult> RemoveTracksFromPlaylistAsync(string spotifyAccessToken, string playlistId, IEnumerable<string> trackSpotifyIds);
        Task<OperationResult> AddSongToPlaylistForSongTagAsync(string spotifyAccessToken, SongTagModel songTag, SongModel song);
        Task<OperationResult> RemoveSongFromPlaylistForSongTagAsync(string spotifyAccessToken, SongTagModel songTag, SongModel song);
        Task<OperationResult> SyncPlaylistForSongTagAsync(string spotifyAccessToken, SongTagModel songTag, IList<SongModel> songs);
        Task<OperationResult> ShufflePlaylistAsync(string spotifyAccessToken, string playlistId);
    }
}
