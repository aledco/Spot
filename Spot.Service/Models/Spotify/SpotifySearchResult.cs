namespace Spot.Business.Models.Spotify
{
    public class SpotifySearchResult
    {
        public SpotifyTrackSearchPage Tracks { get; set; }
        public object Artists { get; set; }
        public object Albums { get; set; }
        public SpotifyPlaylistPage Playlists { get; set; }
    }
}
