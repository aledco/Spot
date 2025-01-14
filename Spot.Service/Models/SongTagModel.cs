namespace Spot.Business.Models
{
    public class SongTagModel
    {
        public int? Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? SpotifyId { get; set; } = null;

        public bool IsPublic { get; set; } = false;

        public string? Description { get; set; }

        public SongTagCategoryModel? Category { get; set; }
    }
}
