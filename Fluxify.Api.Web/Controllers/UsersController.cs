using Fluxify.Application.Models.Client;
using Fluxify.Application.Models.Rest.Subscriber;
using Fluxify.Application.Services.Interfaces;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailAsync([FromRoute] Guid id)
        {
            var result = await UserService.GetDetailAsync(id);

            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateUserRequest model)
        {
            var result = await UserService.CreateAsync(model);

            return Ok(result);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateUserRequest model)
        {
            var result = await UserService.UpdateAsync(model);

            return Ok(result);
        }
    }
}
