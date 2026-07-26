using Fluxify.Application.Models.Common;
using Fluxify.Application.Services.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fluxify.Api.Web.Controllers
{
    [AllowAnonymous]
    public class AuthController : BaseController
    {
        readonly IAuthService _authService;
        public AuthController(CurrentUser currentUser, IAuthService authService) : base(currentUser)
        {
            _authService = authService;
        }

        [HttpPost("api-key/token")]
        public async Task<IActionResult> GetTokenAsync([FromBody] string apiKey)
            => Response(await _authService.GetTokenByApiKeyAsync(apiKey));
            
    }
}
