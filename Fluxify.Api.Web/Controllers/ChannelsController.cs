using Fluxify.Application.Models.Client;
using Fluxify.Application.Models.Rest.Channel;
using Fluxify.Application.Services.Interfaces;
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

            return Response(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateChannelRequest model)
        {
            var result = await channelService.CreateAsync(model);

            return Response(result);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateChannelRequest model)
        {
            var result = await channelService.UpdateAsync(model);

            return Response(result);
        }
    }
}
