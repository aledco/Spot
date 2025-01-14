using Spot.Business.Models;
using Spot.Business.Models.Result;

namespace Spot.Business.Contracts
{
    public interface ISongTagService
    {
        Task<OperationResult<IList<SongTagModel>>> GetAllAsync(string spotifyAccessToken);
        Task<OperationResult<SongTagModel>> GetAsync(string spotifyAccessToken, int songTagId);
        Task<OperationResult<SongTagModel>> SaveAsync(string spotifyAccessToken, SongTagModel model);
        Task<OperationResult> DeleteAsync(string spotifyAccessToken, int songTagId);
    }
}
