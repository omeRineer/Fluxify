using Fluxify.Application.Models.Common;
using Fluxify.Application.Models.Rest.Subscriber;
using Fluxify.Application.Services.Interfaces.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fluxify.Api.Web.Controllers
{
    public class UsersController : BaseController
    {
        readonly IUserService UserService;

        public UsersController(CurrentUser currentUser, IUserService userService) : base(currentUser)
        {
            UserService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync()
            => Response(await UserService.GetListAsync(CurrentUser.NameSpace.Id));

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDetailAsync([FromRoute] Guid id)
            => Response(await UserService.GetDetailAsync(id));

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateUser.Request model)
            => Response(await UserService.CreateAsync(CurrentUser.NameSpace.Id, model));

        [HttpPost("{id:guid}/update")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateUser.Request model)
            => Response(await UserService.UpdateAsync(id, model));
    }
}
