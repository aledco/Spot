using System.Collections.Generic;

namespace Spot.Business.Models.Spotify
{
    public class SpotifyPlaylistPage
    {
        public string Href { get; set; }

        public int Limit { get; set; }

        public string Next { get; set; }

        public int Offset { get; set; }

        public string Previous { get; set; }
        public int Total { get; set; }

        public IEnumerable<SimplifiedSpotifyPlaylist> Items { get; set; } = Enumerable.Empty<SimplifiedSpotifyPlaylist>();
    }
}
