using System.ComponentModel.DataAnnotations;

namespace Spot.Data.Entities
{
    public class User
    {
        public int? Id { get; set; }

        [Required]
        public string? SpotifyId { get; set; }
    }
}
