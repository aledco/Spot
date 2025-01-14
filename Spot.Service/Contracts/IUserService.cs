using Spot.Business.Models;
using Spot.Business.Models.Result;

namespace Spot.Business.Contracts
{
    public interface IUserService
    {
        Task<OperationResult<UserModel>> GetAsync(string spotifyAccessToken);
    }
}
