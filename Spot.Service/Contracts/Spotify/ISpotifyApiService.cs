using Spot.Business.Models.Spotify;

namespace Spot.Business.Contracts.Spotify
{
    public interface ISpotifyApiService
    {
        Task<SpotifyUser> GetSpotifyUserAsync(string spotifyAccessToken);
        Task<IList<(SimplifiedSpotifyPlaylist Playlist, IList<SpotifyTrack> Tracks)>> GetAllTracksFromPlaylistsAsync(string spotifyAccessToken);
        Task<SpotifyPlaylistPage> GetAllAsync(string spotifyAccessToken);
    }
}
