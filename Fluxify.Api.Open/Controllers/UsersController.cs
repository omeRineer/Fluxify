using Fluxify.Application.Models.Common;
using Fluxify.Application.Models.Rest.Subscriber;
using Fluxify.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fluxify.Api.Open.Controllers
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
        {
            var result = await UserService.GetListAsync(CurrentUser.NameSpace.Id);

            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailAsync([FromRoute] Guid id)
        {
            var result = await UserService.GetDetailAsync(id);

            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateUser.Request model)
        {
            var result = await UserService.CreateAsync(CurrentUser.NameSpace.Id, model);

            return Ok(result);
        }

        [HttpPost("{id}/Update")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateUser.Request model)
        {
            var result = await UserService.UpdateAsync(id, model);

            return Ok(result);
        }
    }
}
