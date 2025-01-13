using System.ComponentModel.DataAnnotations;

namespace Spot.Data.Entities
{
    public abstract class BaseIntegerIdEntity
    {
        public int? Id { get; set; }
    }
}
