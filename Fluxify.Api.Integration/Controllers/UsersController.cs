using Fluxify.Application.Models.Common;
using Fluxify.Application.Models.Rest.Subscriber;
using Fluxify.Application.Services.Interfaces.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fluxify.Api.Integration.Controllers
{
    public class UsersController : BaseController
    {
        readonly IUserService UserService;

        public UsersController(CurrentNameSpace currentNameSpace, IUserService userService) : base(currentNameSpace)
        {
            UserService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync()
            => Response(await UserService.GetListAsync(CurrentNameSpace.NameIdentifier));

        [HttpGet("{key}")]
        public async Task<IActionResult> GetDetailAsync([FromRoute] string key)
            => Response(await UserService.GetDetailAsync(CurrentNameSpace.NameIdentifier, key));

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateUser.Request model)
            => Response(await UserService.CreateAsync(CurrentNameSpace.NameIdentifier, model));

        [HttpPost("{key}/update")]
        public async Task<IActionResult> UpdateAsync([FromRoute] string key, [FromBody] UpdateUser.Request model)
            => Response(await UserService.UpdateAsync(CurrentNameSpace.NameIdentifier, key, model));

        [HttpGet("{key}/token")]
        public async Task<IActionResult> GetTokenAsync([FromRoute] string key)
            => Response(await UserService.GetTokenAsync(CurrentNameSpace.NameIdentifier, key));
    }
}
