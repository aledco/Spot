using AutoMapper;
using Spot.Business.Contracts;
using Spot.Business.Contracts.Spotify;
using Spot.Business.Models;
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

        public async Task<UserModel> GetUserAsync(string spotifyAccessToken)
        {
            var spotifyUser = await this._spotifyApiService.GetSpotifyUserAsync(spotifyAccessToken);
            var user = await this._userRepository.GetBySpotifyIdAsync(spotifyUser.Id);
            if (user == null)
            {
                user = await this._userRepository.SaveAsync(new User()
                {
                    SpotifyId = spotifyUser.Id,
                });
            }

            return this._mapper.Map<UserModel>(user);
        }
    }
}
