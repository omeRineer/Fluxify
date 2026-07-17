using Fluxify.Application.Models.Common;
using Fluxify.Application.Services.Interfaces.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fluxify.Api.Open.Controllers
{
    public class TopicsController : BaseController
    {
        readonly ITopicService TopicService;
        public TopicsController(CurrentUser currentUser, ITopicService topicService) : base(currentUser)
        {
            TopicService = topicService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync()
            => Response(await TopicService.GetListAsync());
    }
}
