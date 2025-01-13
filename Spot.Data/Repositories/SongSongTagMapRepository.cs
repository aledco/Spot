using Microsoft.EntityFrameworkCore;
using Spot.Data.Contracts;
using Spot.Data.Entities;

namespace Spot.Data.Repositories
{
    public class SongSongTagMapRepository : BaseRepository<SongSongTagMap>, ISongSongTagMapRepository
    {
        public SongSongTagMapRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<IList<SongSongTagMap>> GetAllBySongIdAsync(int songId)
        {
            return await this._context.SongSongTagMaps
                .Where(e => e.SongId == songId)
                .ToListAsync();
        }
    }
}
