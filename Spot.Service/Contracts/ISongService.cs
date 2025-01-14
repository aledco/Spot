using Spot.Business.Models;
using Spot.Business.Models.Result;

namespace Spot.Business.Contracts
{
    public interface ISongService
    {
        Task<OperationResult<IList<SongModel>>> GetAllAsync(string spotifyAccessToken);
        Task<OperationResult<SongModel>> GetAsync(string spotifyAccessToken, int songId);
        Task<OperationResult<SongModel>> SaveAsync(string spotifyAccessToken, SongModel song);
        Task<OperationResult<IList<SongModel>>> SyncSongsFromPlaylistsAsync(string spotifyAccessToken);
    }
}
