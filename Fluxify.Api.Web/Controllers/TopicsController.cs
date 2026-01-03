using Fluxify.Application.Models.Client;
using Fluxify.Application.Models.Rest.Topic;
using Fluxify.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fluxify.Api.Web.Controllers
{
    public class TopicsController : BaseController
    {
        readonly ITopicService TopicService;
        public TopicsController(CurrentUser currentUser, ITopicService topicService) : base(currentUser)
        {
            TopicService = topicService;
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateTopicRequest req)
        {
            var result = await TopicService.CreateAsync(req);

            return Ok(result);
        }
    }
}
