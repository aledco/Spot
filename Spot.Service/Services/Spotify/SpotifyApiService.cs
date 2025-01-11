using Spot.Business.Contracts.Spotify;
using Spot.Business.Models.Spotify;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Spot.Business.Services.Spotify
{
    public class SpotifyApiService : ISpotifyApiService
    {
        
        protected readonly Uri _spotifyBaseUri = new Uri("https://api.spotify.com/v1");

        protected readonly JsonSerializerOptions _defaultSerializerOptions = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        private HttpClient GetHttpClient(string spotifyAccessToken)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", spotifyAccessToken);
            //httpClient.BaseAddress = this._spotifyBaseUri;
            return httpClient;
        }

        public async Task<SpotifyUser> GetSpotifyUserAsync(string spotifyAccessToken)
        {
            var httpClient = this.GetHttpClient(spotifyAccessToken);
            var response = await httpClient.GetAsync("https://api.spotify.com/v1/me");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<SpotifyUser>(content, this._defaultSerializerOptions);
            return result;
        }

        public async Task<IList<(SimplifiedSpotifyPlaylist Playlist, IList<SpotifyTrack> Tracks)>> GetAllTracksFromPlaylistsAsync(string spotifyAccessToken)
        {
            var playlistResult = await this.GetAllAsync(spotifyAccessToken);
            if (playlistResult == null)
            {
                return null;
            }

            var httpClient = this.GetHttpClient(spotifyAccessToken);
            var playlistTracks = new List<(SimplifiedSpotifyPlaylist Playlist, IList<SpotifyTrack> Tracks)>();
            foreach (var playlist in playlistResult.Items)
            {
                var tracks = new List<SpotifyTrack>();
                SpotifyTrackPage ? page = null;
                var url = $"https://api.spotify.com/v1/playlists/{playlist.Id}/tracks"; ;
                do
                {
                    if (page != null)
                    {
                        url = page.Next;
                    }

                    var response = await httpClient.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                    {
                        return null;
                    }

                    var content = await response.Content.ReadAsStringAsync();
                    page = JsonSerializer.Deserialize<SpotifyTrackPage>(content, this._defaultSerializerOptions);
                    if (page != null)
                    {
                        tracks.AddRange(page.Items
                            .Select(t => t.Track)
                            .Where(t1 => !tracks.Any(t2 => t2.Id == t1.Id)));
                    }
                } while (page != null && page.Next != null);

                playlistTracks.Add((playlist, tracks));
            }

            return playlistTracks;
        }

        // TODO need to handle paging of playlists to really get all
        public async Task<SpotifyPlaylistPage> GetAllAsync(string spotifyAccessToken)
        {
            var user = await this.GetSpotifyUserAsync(spotifyAccessToken);

            var httpClient = this.GetHttpClient(spotifyAccessToken);
            var response = await httpClient.GetAsync($"https://api.spotify.com/v1/users/{user.Id}/playlists");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<SpotifyPlaylistPage>(content, this._defaultSerializerOptions);
            return result;
        }
    }
}
