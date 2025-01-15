namespace Spot.Business.Models.Spotify
{
    public class SpotifyPlaylist : SimplifiedSpotifyPlaylist
    {
        public object Followers { get; set; }

        public new SpotiftyPlaylistTracks Tracks { get; set; }
    }
}
