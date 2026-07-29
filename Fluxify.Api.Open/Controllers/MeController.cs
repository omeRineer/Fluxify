using Fluxify.Application.Models.Common;
using Fluxify.Application.Models.Rest.UserDevice;
using Fluxify.Application.Models.Rest.UserTopic;
using Fluxify.Application.Services;
using Fluxify.Application.Services.Interfaces.Core;
using Fluxify.Application.Services.Interfaces.Mongo;
using Fluxify.Domain.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fluxify.Api.Open.Controllers
{
    public class MeController : BaseController
    {
        readonly IUserService UserService;
        public MeController(CurrentUser currentUser, IUserService UserService) : base(currentUser)
        {
            this.UserService = UserService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfileAsync()
            => Response(await UserService.GetDetailAsync(CurrentUser.NameIdentifier));

    }
}
