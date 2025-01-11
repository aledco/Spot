using System.ComponentModel.DataAnnotations;

namespace Spot.Data.Entities
{
    public class Song
    {
        public int? Id { get; set; }

        [Required]
        public int? UserId { get; set; }

        [Required]
        public string? SpotifyId { get; set; }

        [Required]
        public string? Name {  get; set; }

        [Required]
        public string? Artist { get; set; }

        public virtual User? User { get; set; }

        public virtual IEnumerable<SongTag> SongTags { get; set; } = new List<SongTag>();
    }
}
