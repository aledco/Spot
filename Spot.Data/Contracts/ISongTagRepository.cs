using Spot.Data.Entities;

namespace Spot.Data.Contracts
{
    public interface ISongTagRepository : IRepository<SongTag>
    {
        Task<SongTag?> GetByPlaylistNameAsync(int userId, string playlistName);

        Task<IList<SongTag>> GetAllByUserIdAsync(int userId);
    }
}
