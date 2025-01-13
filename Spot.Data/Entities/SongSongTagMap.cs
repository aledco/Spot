using System.ComponentModel.DataAnnotations;

namespace Spot.Data.Entities
{
    public class SongSongTagMap : BaseIntegerIdEntity
    {
        [Required]
        public int? SongId { get; set; }

        [Required]
        public int? SongTagId { get; set; }

        public virtual Song? Song { get; set; }

        public virtual SongTag? SongTag { get; set; }
    }
}
