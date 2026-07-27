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
        readonly IUserTopicService UserTopicService;
        readonly INotificationService NotificationService;
        readonly IUserDeviceService UserDeviceService;
        public MeController(CurrentUser currentUser, IUserService UserService, IUserDeviceService userDeviceService, IUserTopicService userTopicService, INotificationService notificationService) : base(currentUser)
        {
            this.UserService = UserService;
            UserDeviceService = userDeviceService;
            UserTopicService = userTopicService;
            NotificationService = notificationService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfileAsync()
            => Response(await UserService.GetDetailAsync(CurrentUser.NameIdentifier));

        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotificationsAsync()
            => Response(await NotificationService.GetListAsync(CurrentUser.NameIdentifier));

        [HttpGet("topics")]
        public async Task<IActionResult> GetTopicsAsync()
            => Response(await UserTopicService.GetListAsync(CurrentUser.NameIdentifier));

        [HttpPost("topic/set")]
        public async Task<IActionResult> SetTopicsAsync([FromBody] SetUserTopics.Request req)
            => Response(await UserTopicService.SetAsync(CurrentUser.NameIdentifier,
                                                  CurrentUser.NameSpace.Id,
                                                  req));

        [HttpGet("devices")]
        public async Task<IActionResult> GetDevicesAsync()
            => Response(await UserDeviceService.GetListAsync(CurrentUser.NameIdentifier));


    }
}
