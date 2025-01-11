using System.ComponentModel.DataAnnotations;

namespace Spot.Data.Entities
{
    public class SongTagCategory
    {
        public int? Id { get; set; }

        [Required]
        public int? UserId { get; set; }

        [Required]
        public string? Name { get; set; }

        public virtual User? User { get; set; }
    }
}
