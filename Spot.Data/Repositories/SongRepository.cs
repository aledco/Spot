using Microsoft.EntityFrameworkCore;
using Spot.Data.Contracts;
using Spot.Data.Entities;

namespace Spot.Data.Repositories
{
    public class SongRepository : BaseRepository<Song>, ISongRepository
    {
        public SongRepository(ApplicationContext context) : base(context)
        {   
        }

        public async Task<IList<Song>> GetAllByUserIdAsync(int userId)
        {
            return await this._context.Songs
                .Where(e => e.UserId.HasValue && e.UserId.Value == userId)
                .ToListAsync();
        }

        public async Task<Song?> GetByUserIdAndSpotifyIdAsync(int userId, string spotifyId)
        {
            return await this._context.Songs
                .Where(e => e.UserId.HasValue && e.UserId.Value == userId)
                .Where(e => e.SpotifyId == spotifyId)
                .FirstOrDefaultAsync();
        }
    }
}
