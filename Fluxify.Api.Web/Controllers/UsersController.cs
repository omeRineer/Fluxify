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
    }
}
