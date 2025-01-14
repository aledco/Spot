using Microsoft.EntityFrameworkCore;
using Spot.Data.Contracts;
using Spot.Data.Entities;

namespace Spot.Data.Repositories
{
    public class SongTagRepository : BaseRepository<SongTag>, ISongTagRepository
    {
        public SongTagRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<SongTag?> GetBySpotifyIdAsync(string spotifyId)
        {
            return await this._context.SongTags
                .Where(e => e.SpotifyId == spotifyId)
                .FirstOrDefaultAsync();
        }

        public async Task<IList<SongTag>> GetAllByUserIdAsync(int userId)
        {
            return await this._context.SongTags
                .Where(e => e.UserId.HasValue && e.UserId.Value == userId)
                .Include(e => e.Songs)
                .ToListAsync();
        }
    }
}
