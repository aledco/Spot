using Spot.Business.Models;

namespace Spot.Business.Contracts
{
    public interface ISongService
    {
        Task<IList<SongModel>> GetAllAsync(string spotifyAccessToken);
        Task<IList<SongModel>> SyncAsync(string spotifyAccessToken);
    }
}
