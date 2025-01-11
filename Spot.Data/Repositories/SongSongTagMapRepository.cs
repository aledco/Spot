using Spot.Data.Contracts;
using Spot.Data.Entities;

namespace Spot.Data.Repositories
{
    public class SongSongTagMapRepository : BaseRepository<SongSongTagMap>, ISongSongTagMapRepository
    {
        public SongSongTagMapRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
