namespace Spot.Business.Models
{
    public class SongTagModel
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public SongTagCategoryModel? Category { get; set; }
    }
}
