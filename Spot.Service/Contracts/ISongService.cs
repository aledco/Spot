using Spot.Business.Models;

namespace Spot.Business.Contracts
{
    public interface ISongService
    {
        Task<IList<SongModel>> GetAllAsync(string spotifyAccessToken);

        Task<IList<SongTagModel>> GetAllTagsAsync(string spotifyAccessToken);
        Task<SongModel> GetAsync(string spotifyAccessToken, int songId);
        Task<SongModel> SaveAsync(string spotifyAccessToken, SongModel song);
        Task<IList<SongModel>> SyncAsync(string spotifyAccessToken);
    }
}
