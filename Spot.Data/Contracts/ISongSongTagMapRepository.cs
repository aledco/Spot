using Spot.Data.Entities;

namespace Spot.Data.Contracts
{
    public interface ISongSongTagMapRepository : IRepository<SongSongTagMap>
    {
        Task<IList<SongSongTagMap>> GetAllBySongIdAsync(int songId);
    }
}
