using Spot.Business.Models;

namespace Spot.Business.Contracts
{
    public interface ISongTagService
    {
        Task<IList<SongTagModel>> GetAllAsync(string spotifyAccessToken);
        Task<SongTagModel> GetAsync(string spotifyAccessToken, int songTagId);
        Task<SongTagModel> SaveAsync(string spotifyAccessToken, SongTagModel model);
    }
}
