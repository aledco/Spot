using Microsoft.AspNetCore.Mvc;
using Spot.Business.Models.Configuration;
using Spot.Business.Models.Result;

namespace Spot.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConfigurationController : BaseController
    {
        private readonly IConfiguration _configuration;

        public ConfigurationController(IConfiguration configuration) 
        {
            this._configuration = configuration;
        }

        [HttpGet]
        [Route("")]
        public async Task<OperationResult<ClientAppSettingsModel>> GetAsync()
        {
            var section = this._configuration.GetSection("ClientAppSettings");
            if (section == null)
            {
                return OperationResult<ClientAppSettingsModel>.Fatal();
            }

            var settings = section.Get<ClientAppSettingsModel>();
            if (settings == null || settings.SpotifySettings == null)
            {
                return OperationResult<ClientAppSettingsModel>.Fatal();
            }

            return OperationResult<ClientAppSettingsModel>.Success(settings);
        }
    }
}
