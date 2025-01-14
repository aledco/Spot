using System.ComponentModel.DataAnnotations;

namespace Spot.Data.Entities
{
    public class SongTag : BaseIntegerIdEntity
    {
        public int? SongCategoryId { get; set; } = null;

        [Required]
        public int? UserId { get; set; }

        public string? SpotifyId { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public bool? IsPublic { get; set; }

        public string? Description { get; set; }

        public virtual SongTagCategory? SongTagCategory { get; set; }

        public virtual User? User { get; set; }

        public virtual List<Song> Songs { get; set; } = [];

        public virtual List<SongSongTagMap> SongSongTagMaps { get; set; } = [];
    }
}
