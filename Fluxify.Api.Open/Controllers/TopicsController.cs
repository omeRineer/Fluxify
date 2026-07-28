using Fluxify.Application.Models.Common;
using Fluxify.Application.Models.Rest.UserTopic;
using Fluxify.Application.Services.Core;
using Fluxify.Application.Services.Interfaces.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fluxify.Api.Open.Controllers
{
    public class TopicsController : BaseController
    {
        readonly ITopicService TopicService;
        readonly IUserTopicService _userTopicService;
        public TopicsController(CurrentUser currentUser, ITopicService topicService, IUserTopicService userTopicService) : base(currentUser)
        {
            TopicService = topicService;
            _userTopicService = userTopicService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync()
            => Response(await TopicService.GetListAsync());

        [HttpPost("topic/set")]
        public async Task<IActionResult> SetTopicsAsync([FromBody] SetUserTopics.Request req)
            => Response(await _userTopicService.SetAsync(CurrentUser.NameIdentifier,
                                                  CurrentUser.NameSpace.Id,
                                                  req));
    }
}
