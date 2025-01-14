using AutoMapper;
using Spot.Business.Contracts;
using Spot.Business.Contracts.Spotify;
using Spot.Business.Models;
using Spot.Business.Models.Result;
using Spot.Data.Contracts;
using Spot.Data.Entities;

namespace Spot.Business.Services
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISpotifyApiService _spotifyApiService;

        public UserService(
            IMapper mapper,
            IUserRepository userRepository, 
            ISpotifyApiService spotifyApiService)
            : base(mapper)
        {
            this._userRepository = userRepository;
            this._spotifyApiService = spotifyApiService;
        }

        public async Task<OperationResult<UserModel>> GetAsync(string spotifyAccessToken)
        {
            var spotifyUserResult = await this._spotifyApiService.GetSpotifyUserAsync(spotifyAccessToken);
            if (!spotifyUserResult.IsValid)
            {
                return spotifyUserResult.ErrorsAs<UserModel>();
            }

            var spotifyUser = spotifyUserResult.Result;
            var user = await this._userRepository.GetBySpotifyIdAsync(spotifyUser.Id);
            if (user == null)
            {
                user = await this._userRepository.SaveAsync(new User()
                {
                    SpotifyId = spotifyUser.Id,
                });
            }

            return OperationResult<UserModel>.Success(this._mapper.Map<UserModel>(user));
        }
    }
}
