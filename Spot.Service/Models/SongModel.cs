namespace Spot.Business.Models
{
    public class SongModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Artist { get; set; }

        public IList<SongTagModel> Tags { get; set; } = new List<SongTagModel>();
    }
}
