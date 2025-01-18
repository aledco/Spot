using Spot.Business.Models;
using Spot.Business.Models.Result;

namespace Spot.Business.Contracts
{
    public interface ISongService
    {
        Task<OperationResult<IList<SongModel>>> GetAllAsync();
        Task<OperationResult<SongModel>> GetAsync(int songId);
        Task<OperationResult<SongModel>> SaveAsync(SongModel song);
        Task<OperationResult> DeleteAsync(int songId);
        Task<OperationResult<IList<SongModel>>> SearchAsync(SongSearchCriteriaModel searchCriteria);
        Task<OperationResult<IList<SongModel>>> SyncSongsFromPlaylistsAsync();
    }
}
