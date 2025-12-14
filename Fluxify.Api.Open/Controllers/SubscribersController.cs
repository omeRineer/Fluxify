using Fluxify.Application.Models.Client;
using Fluxify.Application.Models.Rest.Subscriber;
using Fluxify.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fluxify.Api.Open.Controllers
{
    [Authorize(AuthenticationSchemes = "FluxifySecurityScheme")]
    public class SubscribersController : BaseController
    {
        readonly ISubscriberService SubscriberService;

        public SubscribersController(CurrentUser currentUser, ISubscriberService subscriberService) : base(currentUser)
        {
            SubscriberService = subscriberService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSubscribersAsync()
        {
            var result = await SubscriberService.GetSubscribersByNameSpaceAsync(CurrentUser.NameSpace);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailAsync([FromRoute] Guid id)
        {
            var result = await SubscriberService.GetDetailAsync(id);

            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateSubscriberRequest model)
        {
            var result = await SubscriberService.CreateAsync(model);

            return Ok(result);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateSubscriberRequest model)
        {
            var result = await SubscriberService.UpdateAsync(model);

            return Ok(result);
        }
    }
}
