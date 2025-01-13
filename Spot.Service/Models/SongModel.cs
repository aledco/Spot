namespace Spot.Business.Models
{
    public class SongModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Artist { get; set; } = string.Empty;

        public string SpotifyId { get; set; } = string.Empty;

        public IList<int> TagIds { get; set; } = [];

        public IList<string> Tags { get; set; } = [];
    }
}
