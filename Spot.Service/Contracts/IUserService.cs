using Spot.Business.Models;

namespace Spot.Business.Contracts
{
    public interface IUserService
    {
        Task<UserModel> GetUserAsync(string spotifyAccessToken);
    }
}
