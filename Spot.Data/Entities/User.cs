using System.ComponentModel.DataAnnotations;

namespace Spot.Data.Entities
{
    public class User : BaseIntegerIdEntity
    {
        [Required]
        public string? SpotifyId { get; set; }
    }
}
