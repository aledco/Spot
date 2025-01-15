namespace Spot.Business.Models.Spotify
{
    public class SpotiftyPlaylistTracks
    {
        public string Next {  get; set; }
        public int Limit { get; set; }
        public int Offset { get; set; }
        public int Total { get; set; }
        public SpotifyPlaylistTrack[] Items = [];
    }
}
