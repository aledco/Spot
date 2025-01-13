namespace Spot.Business.Models
{
    public class SongTagModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? SpotifyId { get; set; } = null;

        public SongTagCategoryModel? Category { get; set; }
    }
}
