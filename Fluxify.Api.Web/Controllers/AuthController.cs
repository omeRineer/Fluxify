using Fluxify.Application.Models.Common;
using Fluxify.Application.Models.Rest.Auth;
using Fluxify.Application.Services.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fluxify.Api.Web.Controllers
{
    
    public class AuthController : BaseController
    {
        readonly IAuthService _authService;
        public AuthController(CurrentUser currentUser, IAuthService authService) : base(currentUser)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] Login.Request req)
            => Response(await _authService.LoginAsync(req));

        [HttpPost("check/password")]
        public async Task<IActionResult> CheckByPasswordAsync(string password)
            => Response(await _authService.CheckUserAsync(CurrentUser.NameIdentifier, password));

    }
}
