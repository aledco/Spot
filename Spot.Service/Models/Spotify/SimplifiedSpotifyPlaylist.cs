namespace Spot.Business.Models.Spotify
{
    public class SimplifiedSpotifyPlaylist
    {
        public bool Collaborative { get; set; }

        public string Description { get; set; }

        public object ExternalUrls { get; set; }

        public string Href { get; set; }

        public string Id { get; set; }

        public object[] Images { get; set; } // TODO

        public string Name { get; set; }

        public object Owner { get; set; }

        public bool Public { get; set; }

        public string SnapshotId { get; set; }

        public object Tracks { get; set; } // TODO

        public string Type { get; set; }

        public string Uri { get; set; }
    }
}
