using Fluxify.Application.Models.Common;
using Fluxify.Application.Models.Rest.Channel;
using Fluxify.Application.Services.Interfaces.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fluxify.Api.Web.Controllers
{
    public class ChannelsController : BaseController
    {
        readonly IChannelService channelService;

        public ChannelsController(CurrentUser currentUser, IChannelService channelService) : base(currentUser)
        {
            this.channelService = channelService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailAsync([FromRoute] Guid id)
        {
            var result = await channelService.GetDetailAsync(id);

            return Ok(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateChannel.Request model)
        {
            var result = await channelService.CreateAsync(model);

            return Ok(result);
        }

        [HttpPost("Update/{id}")]
        public async Task<IActionResult> UpdateAsync([FromRoute] Guid id,
                                                     [FromBody] UpdateChannel.Request model)
        {
            var result = await channelService.UpdateAsync(id, model);

            return Ok(result);
        }
    }
}
