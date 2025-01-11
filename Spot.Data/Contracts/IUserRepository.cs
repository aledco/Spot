using Spot.Data.Entities;

namespace Spot.Data.Contracts
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetBySpotifyIdAsync(string spotifyId);
    }
}
