using System.ComponentModel.DataAnnotations;

namespace Spot.Data.Entities
{
    public class SongSongTagMap
    {
        public int? Id { get; set; }

        [Required]
        public int? SongId { get; set; }

        [Required]
        public int? SongTagId { get; set; }

        public virtual Song? Song { get; set; }

        public virtual SongTag? SongTag { get; set; }
    }
}
