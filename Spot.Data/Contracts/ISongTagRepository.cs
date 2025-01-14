using Spot.Data.Entities;

namespace Spot.Data.Contracts
{
    public interface ISongTagRepository : IRepository<SongTag>
    {
        Task<SongTag?> GetBySpotifyIdAsync(string spotifyId);

        Task<IList<SongTag>> GetAllByUserIdAsync(int userId);
    }
}
