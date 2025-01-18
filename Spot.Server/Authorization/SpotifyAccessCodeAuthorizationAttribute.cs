using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Spot.Server.Authorization
{
    public class SpotifyAccessCodeAuthorizationAttribute : TypeFilterAttribute
    {
        public SpotifyAccessCodeAuthorizationAttribute() : base(typeof(SpotifyAccessCodeAuthorizationFilter))
        {
        }
    }
}
