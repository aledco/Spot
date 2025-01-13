using Azure;
using Spot.Business.Contracts.Spotify;
using Spot.Business.Models;
using Spot.Business.Models.Spotify;
using Spot.Data.Entities;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
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

        public async Task<SpotifySearchResult> SearchAsync(string spotifyAccessToken, SpotifySearchCriteria searchCriteria)
        {
            var queryString = searchCriteria.BuildQueryString();
            var httpClient = this.GetHttpClient(spotifyAccessToken);
            var response = await httpClient.GetAsync("https://api.spotify.com/v1/search" + queryString);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<SpotifySearchResult>(content, this._defaultSerializerOptions);
            return result;
        }

        // TODO rename as get page of playlists
        public async Task<SpotifyPlaylistPage> GetAllPlaylistsAsync(string spotifyAccessToken)
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

            // TODO following line is for testing only
            result.Items = result.Items.Where(p => p.Name.StartsWith("Test"));

            return result;
        }

        public async Task<IList<(SimplifiedSpotifyPlaylist Playlist, IList<SpotifyTrack> Tracks)>> GetAllTracksFromPlaylistsAsync(string spotifyAccessToken)
        {
            var playlistResult = await this.GetAllPlaylistsAsync(spotifyAccessToken);
            if (playlistResult == null)
            {
                return null;
            }

            var httpClient = this.GetHttpClient(spotifyAccessToken);
            var playlistTracks = new List<(SimplifiedSpotifyPlaylist Playlist, IList<SpotifyTrack> Tracks)>();
            foreach (var playlist in playlistResult.Items)
            {
                var tracks = await this.GetAllTracksForPlaylistAsync(spotifyAccessToken, playlist.Id);
                playlistTracks.Add((playlist, tracks));
            }

            return playlistTracks;
        }

        public async Task<IList<SpotifyTrack>> GetAllTracksForPlaylistAsync(string spotifyAccessToken, string playlistId)
        {
            var httpClient = this.GetHttpClient(spotifyAccessToken);

            var tracks = new List<SpotifyTrack>();
            SpotifyTrackPage? page = null;
            var url = $"https://api.spotify.com/v1/playlists/{playlistId}/tracks"; ;
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

            return tracks;
        }

        public async Task<SpotifyPlaylist?> GetPlaylistAsync(string spotifyAccessToken, string? playlistId)
        {
            if (string.IsNullOrEmpty(playlistId))
            {
                return null;
            }

            var httpClient = this.GetHttpClient(spotifyAccessToken);
            var response = await httpClient.GetAsync($"https://api.spotify.com/v1/playlists/{playlistId}");
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<SpotifyPlaylist>(content, this._defaultSerializerOptions);
            return result;
        }

        public async Task<SpotifyPlaylist> CreatePlaylistFromSongTagAsync(string spotifyAccessToken, SongTagModel songTag)
        {
            var user = await this.GetSpotifyUserAsync(spotifyAccessToken);

            var playlist = new
            {
                Name = songTag.Name,
                Public = true, // TODO add the ability to mark a tag as private
                Collaborative = false,
                Description = string.Empty, // TODO add the ability to give a tag a description
            };

            var playlistContent = new StringContent(JsonSerializer.Serialize(playlist, this._defaultSerializerOptions));

            var httpClient = this.GetHttpClient(spotifyAccessToken);
            var response = await httpClient.PostAsync($"https://api.spotify.com/v1/users/{user.Id}/playlists", playlistContent);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<SpotifyPlaylist>(content, this._defaultSerializerOptions);
            return result;
        }

        public async Task AddTracksToPlaylistAsync(string spotifyAccessToken, string playlistId, IEnumerable<string> trackSpotifyIds)
        {
            var body = new
            {
                Uris = trackSpotifyIds.Select(id => "spotify:track:" + id)
            };

            var content = new StringContent(JsonSerializer.Serialize(body, this._defaultSerializerOptions));

            var httpClient = this.GetHttpClient(spotifyAccessToken);
            var response = await httpClient.PostAsync($"https://api.spotify.com/v1/playlists/{playlistId}/tracks", content);
            if (!response.IsSuccessStatusCode)
            {
                return; // TODO return error
            }
        }

        public async Task RemoveTracksFromPlaylistAsync(string spotifyAccessToken, string playlistId, IEnumerable<string> trackSpotifyIds)
        {
            var body = new
            {
                Tracks = trackSpotifyIds.Select(id => new 
                { 
                    Uri = "spotify:track:" + id 
                })
            };

            var content = new StringContent(JsonSerializer.Serialize(body, this._defaultSerializerOptions));

            var httpClient = this.GetHttpClient(spotifyAccessToken);
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Delete,
                RequestUri = new Uri($"https://api.spotify.com/v1/playlists/{playlistId}/tracks"),
                Content = content
            };

            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                return; // TODO return error
            }
        }

        public async Task AddSongToPlaylistForSongTagAsync(string spotifyAccessToken, SongTagModel songTag, SongModel song)
        {
            var playlist = await this.GetPlaylistAsync(spotifyAccessToken, songTag.SpotifyId);
            if (playlist == null)
            {
                return; // TODO return error
            }

            await this.AddTracksToPlaylistAsync(spotifyAccessToken, playlist.Id, [song.SpotifyId]);
        }

        public async Task RemoveSongFromPlaylistForSongTagAsync(string spotifyAccessToken, SongTagModel songTag, SongModel song)
        {
            var playlist = await this.GetPlaylistAsync(spotifyAccessToken, songTag.SpotifyId);
            if (playlist == null)
            {
                return; // TODO return error
            }

            await this.RemoveTracksFromPlaylistAsync(spotifyAccessToken, playlist.Id, [song.SpotifyId]);
        }

        public async Task SyncPlaylistForSongTagAsync(string spotifyAccessToken, SongTagModel songTag, IList<SongModel> songs)
        {
            var playlist = await this.GetPlaylistAsync(spotifyAccessToken, songTag.SpotifyId);
            if (playlist == null)
            {
                return; // TODO return error
            }

            // TODO create the playlist when the tag is created instead
            //if (playlist == null)
            //{
            //    playlist = await this.CreatePlaylistFromSongTagAsync(spotifyAccessToken, songTag);
            //    songTag.SpotifyId = playlist.Id;
            //    await this.AddTracksToPlaylistAsync(spotifyAccessToken, playlist.Id, songs.Select(s => s.SpotifyId));
            //}

            var existingTracks = await GetAllTracksForPlaylistAsync(spotifyAccessToken, playlist.Id);

            var tracksToAdd = songs
                .Select(s => s.SpotifyId)
                .Where(id => !existingTracks.Any(track => track.Id == id));

            var tracksToRemove = existingTracks
                .Select(t => t.Id)
                .Where(id => !songs.Any(s => s.SpotifyId == id));

            await this.AddTracksToPlaylistAsync(spotifyAccessToken, playlist.Id, tracksToAdd);
            await this.RemoveTracksFromPlaylistAsync(spotifyAccessToken, playlist.Id, tracksToRemove);
        }
    }
}
