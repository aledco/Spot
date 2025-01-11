using Microsoft.AspNetCore.Mvc;

namespace Spot.Server.Controllers
{
    public abstract class BaseController : Controller
    {
        protected string? SpotifyAccessToken { get => this.Request.Query["spotifyAccessToken"]; }
    }
}
