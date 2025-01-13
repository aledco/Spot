namespace Spot.Business.Models.Spotify
{
    public class SpotifyType
    {
        public string Value { get; private set; }
        private SpotifyType(string value) { Value = value; }

        public override string ToString()
        {
            return this.Value;
        }

        public static SpotifyType Album { get => new SpotifyType("album"); }
        public static SpotifyType Artist { get => new SpotifyType("artist"); }
        public static SpotifyType Playlist { get => new SpotifyType("playlist"); }
        public static SpotifyType Track { get => new SpotifyType("track"); }
    }
}
