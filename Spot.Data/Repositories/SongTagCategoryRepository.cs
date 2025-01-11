using Spot.Data.Contracts;
using Spot.Data.Entities;

namespace Spot.Data.Repositories
{
    public class SongTagCategoryRepository : BaseRepository<SongTagCategory>, ISongTagCategoryRepository
    {
        public SongTagCategoryRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
