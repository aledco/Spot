using Spot.Data.Entities;

namespace Spot.Data.Contracts
{
    public interface ISongRepository : IRepository<Song>
    {
        Task<IList<Song>> GetAllByUserIdAsync(int userId);

        Task<Song?> GetByUserIdAndSpotifyIdAsync(int userId, string spotifyId);
    }
}
