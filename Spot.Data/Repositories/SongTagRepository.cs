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

        public async Task<SongTag?> GetByPlaylistNameAsync(int userId, string playlistName)
        {
            return await this._context.SongTags
                .Where(e => e.UserId.HasValue && e.UserId.Value == userId)
                .Where(e => e.Name == playlistName)
                .FirstOrDefaultAsync();
        }
    }
}
