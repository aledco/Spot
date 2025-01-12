using System.ComponentModel.DataAnnotations;

namespace Spot.Data.Entities
{
    public class SongTag
    {
        public int? Id { get; set; }

        public int? SongCategoryId { get; set; } = null;

        [Required]
        public int? UserId { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        public bool? Active { get; set; } = true;

        public virtual SongTagCategory? SongTagCategory { get; set; }

        public virtual User? User { get; set; }

        public virtual List<Song> Songs { get; set; } = [];

        public virtual List<SongSongTagMap> SongSongTagMaps { get; set; } = [];
    }
}
