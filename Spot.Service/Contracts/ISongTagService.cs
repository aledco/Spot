using Spot.Business.Models;
using Spot.Business.Models.Result;

namespace Spot.Business.Contracts
{
    public interface ISongTagService
    {
        Task<OperationResult<IList<SongTagModel>>> GetAllAsync();
        Task<OperationResult<SongTagModel>> GetAsync(int songTagId);
        Task<OperationResult<SongTagModel>> SaveAsync(SongTagModel model);
        Task<OperationResult> DeleteAsync(int songTagId);
    }
}
