using Azure;
using Spot.Business.Contracts.Spotify;
using Spot.Business.Models;
using Spot.Business.Models.Result;
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

        public async Task<OperationResult<SpotifyUser>> GetSpotifyUserAsync(string spotifyAccessToken)
        {
            var httpClient = this.GetHttpClient(spotifyAccessToken);
            var response = await httpClient.GetAsync("https://api.spotify.com/v1/me");
            if (!response.IsSuccessStatusCode)
            {
                return OperationResult<SpotifyUser>.Fatal();
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<SpotifyUser>(content, this._defaultSerializerOptions);
            if (result == null)
            {
                return OperationResult<SpotifyUser>.Fatal();
            }

            return OperationResult<SpotifyUser>.Success(result);
        }

        public async Task<OperationResult<SpotifySearchResult>> SearchAsync(string spotifyAccessToken, SpotifySearchCriteria searchCriteria)
        {
            var queryString = searchCriteria.BuildQueryString();
            var httpClient = this.GetHttpClient(spotifyAccessToken);
            var response = await httpClient.GetAsync("https://api.spotify.com/v1/search" + queryString);
            if (!response.IsSuccessStatusCode)
            {
                return OperationResult<SpotifySearchResult>.Fatal();
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<SpotifySearchResult>(content, this._defaultSerializerOptions);
            if (result == null)
            {
                return OperationResult<SpotifySearchResult>.Fatal();
            }

            return OperationResult<SpotifySearchResult>.Success(result);
        }

        public async Task<OperationResult<IList<SimplifiedSpotifyPlaylist>>> GetAllPlaylistsAsync(string spotifyAccessToken)
        {
            var userResult = await this.GetSpotifyUserAsync(spotifyAccessToken);
            if (!userResult.IsValid)
            {
                return userResult.ErrorsAs<IList<SimplifiedSpotifyPlaylist>>();
            }

            var user = userResult.Result;

            var httpClient = this.GetHttpClient(spotifyAccessToken);

            var playlists = new List<SimplifiedSpotifyPlaylist>();
            SpotifyPlaylistPage? page = null;
            var url = $"https://api.spotify.com/v1/users/{user.Id}/playlists"; ;
            do
            {
                if (page != null)
                {
                    url = page.Next;
                }

                var response = await httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    return OperationResult<IList<SimplifiedSpotifyPlaylist>>.Failed();
                }

                var content = await response.Content.ReadAsStringAsync();
                page = JsonSerializer.Deserialize<SpotifyPlaylistPage>(content, this._defaultSerializerOptions);
                if (page != null)
                {
                    playlists.AddRange(page.Items);
                }
            } while (page != null && page.Next != null);

            playlists = playlists.Where(p => p.Name.StartsWith("Test")).ToList();

            return OperationResult<IList<SimplifiedSpotifyPlaylist>>.Success(playlists);
        }

        public async Task<OperationResult<IList<(SimplifiedSpotifyPlaylist Playlist, IList<SpotifyTrack> Tracks)>>> GetAllTracksFromPlaylistsAsync(string spotifyAccessToken)
        {
            var playlistResult = await this.GetAllPlaylistsAsync(spotifyAccessToken);
            if (!playlistResult.IsValid)
            {
                return playlistResult.ErrorsAs<IList<(SimplifiedSpotifyPlaylist Playlist, IList<SpotifyTrack> Tracks)>>();
            }

            var playlists = playlistResult.Result;
         
            var httpClient = this.GetHttpClient(spotifyAccessToken);
            var playlistTracks = new List<(SimplifiedSpotifyPlaylist Playlist, IList<SpotifyTrack> Tracks)>();
            foreach (var playlist in playlists)
            {
                var tracksResult = await this.GetAllTracksForPlaylistAsync(spotifyAccessToken, playlist.Id);
                if (!tracksResult.IsValid)
                {
                    return tracksResult.ErrorsAs<IList<(SimplifiedSpotifyPlaylist Playlist, IList<SpotifyTrack> Tracks)>>();
                }

                playlistTracks.Add((playlist, tracksResult.Result));
            }

            return OperationResult<IList<(SimplifiedSpotifyPlaylist Playlist, IList<SpotifyTrack> Tracks)>>.Success(playlistTracks);
        }

        public async Task<OperationResult<IList<SpotifyTrack>>> GetAllTracksForPlaylistAsync(string spotifyAccessToken, string playlistId)
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
                    return OperationResult<IList<SpotifyTrack>>.Failed();
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

            return OperationResult<IList<SpotifyTrack>>.Success(tracks);
        }

        public async Task<OperationResult<SpotifyPlaylist?>> GetPlaylistAsync(string spotifyAccessToken, string? playlistId)
        {
            if (string.IsNullOrEmpty(playlistId))
            {
                return OperationResult<SpotifyPlaylist?>.Success(null);
            }

            var httpClient = this.GetHttpClient(spotifyAccessToken);
            var response = await httpClient.GetAsync($"https://api.spotify.com/v1/playlists/{playlistId}");
            if (!response.IsSuccessStatusCode)
            {
                return OperationResult<SpotifyPlaylist?>.Failed();
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<SpotifyPlaylist>(content, this._defaultSerializerOptions);
            return OperationResult<SpotifyPlaylist?>.Success(result);
        }

        public async Task<OperationResult> DeletePlaylistAsync(string spotifyAccessToken, string playlistId)
        {
            if (string.IsNullOrEmpty(playlistId))
            {
                return OperationResult.Success();
            }

            var httpClient = this.GetHttpClient(spotifyAccessToken);
            var response = await httpClient.DeleteAsync($"https://api.spotify.com/v1/playlists/{playlistId}/followers");
            if (!response.IsSuccessStatusCode)
            {
                return OperationResult.Failed();
            }

            return OperationResult.Success();
        }

        public async Task<OperationResult<SpotifyPlaylist>> CreatePlaylistFromSongTagAsync(string spotifyAccessToken, SongTagModel songTag)
        {
            var userResult = await this.GetSpotifyUserAsync(spotifyAccessToken);
            if (!userResult.IsValid)
            {
                return userResult.ErrorsAs<SpotifyPlaylist>();
            }

            var user = userResult.Result;

            var playlist = new
            {
                songTag.Name,
                songTag.Description,
                Public = songTag.IsPublic,
                Collaborative = false,
            };

            var playlistContent = new StringContent(JsonSerializer.Serialize(playlist, this._defaultSerializerOptions));

            var httpClient = this.GetHttpClient(spotifyAccessToken);
            var response = await httpClient.PostAsync($"https://api.spotify.com/v1/users/{user.Id}/playlists", playlistContent);
            if (!response.IsSuccessStatusCode)
            {
                return OperationResult<SpotifyPlaylist>.Failed();
            }

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<SpotifyPlaylist>(content, this._defaultSerializerOptions);
            if (result == null)
            {
                return OperationResult<SpotifyPlaylist>.Failed();
            }

            return OperationResult<SpotifyPlaylist>.Success(result);
        }

        public async Task<OperationResult> UpdatePlaylistFromSongTagAsync(string spotifyAccessToken, SongTagModel songTag)
        {
            var user = await this.GetSpotifyUserAsync(spotifyAccessToken);

            var playlist = new
            {
                songTag.Name,
                songTag.Description,
                Public = songTag.IsPublic,
                Collaborative = false,
            };

            var playlistContent = new StringContent(JsonSerializer.Serialize(playlist, this._defaultSerializerOptions));

            var httpClient = this.GetHttpClient(spotifyAccessToken);
            var response = await httpClient.PutAsync($"https://api.spotify.com/v1/playlists/{songTag.SpotifyId}", playlistContent);
            if (!response.IsSuccessStatusCode)
            {
                return OperationResult.Failed();
            }

            return OperationResult.Success();
        }
        
        public async Task<OperationResult> AddTracksToPlaylistAsync(string spotifyAccessToken, string playlistId, IEnumerable<string> trackSpotifyIds)
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
                return OperationResult.Failed();
            }

            return OperationResult.Success();
        }

        public async Task<OperationResult> RemoveTracksFromPlaylistAsync(string spotifyAccessToken, string playlistId, IEnumerable<string> trackSpotifyIds)
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
                return OperationResult.Failed();
            }

            return OperationResult.Success();
        }

        public async Task<OperationResult> AddSongToPlaylistForSongTagAsync(string spotifyAccessToken, SongTagModel songTag, SongModel song)
        {
            var playlistResult = await this.GetPlaylistAsync(spotifyAccessToken, songTag.SpotifyId);
            if (!playlistResult.IsValid)
            {
                return OperationResult.ErrorsFrom(playlistResult);
            }

            var playlist = playlistResult.Result;
            if (playlist == null)
            {
                return OperationResult.Failed();
            }

            return await this.AddTracksToPlaylistAsync(spotifyAccessToken, playlist.Id, [song.SpotifyId]);
        }

        public async Task<OperationResult> RemoveSongFromPlaylistForSongTagAsync(string spotifyAccessToken, SongTagModel songTag, SongModel song)
        {
            var playlistResult = await this.GetPlaylistAsync(spotifyAccessToken, songTag.SpotifyId);
            if (!playlistResult.IsValid)
            {
                return OperationResult.ErrorsFrom(playlistResult);
            }

            var playlist = playlistResult.Result;
            if (playlist == null)
            {
                return OperationResult.Failed();
            }

            return await this.RemoveTracksFromPlaylistAsync(spotifyAccessToken, playlist.Id, [song.SpotifyId]);
        }

        public async Task<OperationResult> SyncPlaylistForSongTagAsync(string spotifyAccessToken, SongTagModel songTag, IList<SongModel> songs)
        {
            var playlistResult = await this.GetPlaylistAsync(spotifyAccessToken, songTag.SpotifyId);
            if (!playlistResult.IsValid)
            {
                return OperationResult.ErrorsFrom(playlistResult);
            }

            var playlist = playlistResult.Result;
            if (playlist == null)
            {
                return OperationResult.Failed();
            }

            var existingTracksResult = await GetAllTracksForPlaylistAsync(spotifyAccessToken, playlist.Id);
            if (!existingTracksResult.IsValid)
            {
                return OperationResult.ErrorsFrom(existingTracksResult);
            }

            var existingTracks = existingTracksResult.Result;

            var tracksToAdd = songs
                .Select(s => s.SpotifyId)
                .Where(id => !existingTracks.Any(track => track.Id == id));

            var tracksToRemove = existingTracks
                .Select(t => t.Id)
                .Where(id => !songs.Any(s => s.SpotifyId == id));

            await this.AddTracksToPlaylistAsync(spotifyAccessToken, playlist.Id, tracksToAdd);
            await this.RemoveTracksFromPlaylistAsync(spotifyAccessToken, playlist.Id, tracksToRemove);
            return OperationResult.Success();
        }

        public async Task<OperationResult> ShufflePlaylistAsync(string spotifyAccessToken, string playlistId)
        {
            var playlistResult = await this.GetPlaylistAsync(spotifyAccessToken, playlistId);
            if (!playlistResult.IsValid)
            {
                return OperationResult.ErrorsFrom(playlistResult);   
            }

            var playlist = playlistResult.Result;
            var size = playlist.Tracks.Total;

            var httpClient = this.GetHttpClient(spotifyAccessToken);

            var rand = new Random();
            for (var i = 0; i < size; i++)
            {
                var newPosition = rand.Next(size+1);
                var body = new
                {
                    RangeStart = i,
                    InsertBefore = newPosition,
                    RangeLengt = 1
                };

                var bodyContent = new StringContent(JsonSerializer.Serialize(body, this._defaultSerializerOptions));
                await httpClient.PutAsync($"https://api.spotify.com/v1/playlists/{playlistId}/tracks", bodyContent);
            }

            return OperationResult.Success();
        }
    }
}
