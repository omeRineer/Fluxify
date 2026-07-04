using Fluxify.Application.Models.Common;
using Fluxify.Application.Models.Rest.UserDevice;
using Fluxify.Application.Models.Rest.UserTopic;
using Fluxify.Application.Services;
using Fluxify.Application.Services.Interfaces;
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
        public MeController(CurrentUser currentUser, IUserService UserService, INotificationService notificationService, IUserDeviceService userDeviceService, IUserTopicService userTopicService) : base(currentUser)
        {
            this.UserService = UserService;
            NotificationService = notificationService;
            UserDeviceService = userDeviceService;
            UserTopicService = userTopicService;
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> GetProfileAsync()
            => Response(await UserService.GetDetailAsync(CurrentUser.NameIdentifier));

        [HttpGet("Notifications")]
        public async Task<IActionResult> GetNotificationsAsync()
            => Response(await NotificationService.GetListAsync(CurrentUser.NameIdentifier));

        [HttpGet("Topics")]
        public async Task<IActionResult> GetTopicsAsync()
            => Response(await UserTopicService.GetListAsync(CurrentUser.NameIdentifier));

        [HttpGet("Devices")]
        public async Task<IActionResult> GetDevicesAsync()
            => Response(await UserDeviceService.GetListAsync(CurrentUser.NameIdentifier));



        [HttpPost("Device/Set")]
        public async Task<IActionResult> SetDeviceAsync([FromBody] SetUserDevice.Request req)
            => Response(await UserDeviceService.SetAsync(CurrentUser.NameIdentifier,
                                                   CurrentUser.NameSpace.Id,
                                                   req));

        [HttpPost("Topic/Set")]
        public async Task<IActionResult> SetTopicsAsync([FromBody] SetUserTopics.Request req)
            => Response(await UserTopicService.SetAsync(CurrentUser.NameIdentifier,
                                                  CurrentUser.NameSpace.Id,
                                                  req));
    }
}
