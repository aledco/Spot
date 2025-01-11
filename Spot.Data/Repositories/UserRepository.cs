using Microsoft.EntityFrameworkCore;
using Spot.Data.Contracts;
using Spot.Data.Entities;

namespace Spot.Data.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<User?> GetBySpotifyIdAsync(string spotifyId)
        {
            return await this._context.Users
                .Where(e => e.SpotifyId == spotifyId)
                .FirstOrDefaultAsync();
        }
    }
}
