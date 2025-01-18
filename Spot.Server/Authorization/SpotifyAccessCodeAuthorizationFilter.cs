using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Spot.Business.Models.Result;

namespace Spot.Server.Authorization
{
    public class SpotifyAccessCodeAuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.Request.Headers.TryGetValue("Spotify-Access-Token", out var token))
            {
                if (string.IsNullOrEmpty(token))
                {
                    context.Result = OperationResult.Fatal();
                }
            }
            else
            {
                context.Result = OperationResult.Fatal();
            }
        }
    }
}
