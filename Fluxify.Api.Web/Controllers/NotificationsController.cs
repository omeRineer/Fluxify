using Fluxify.Application.Models.Common;
using Fluxify.Application.Models.Common.Pagination;
using Fluxify.Application.Models.Rest.Notification;
using Fluxify.Application.Services.Interfaces.Mongo;
using Fluxify.Application.Services.Mongo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fluxify.Api.Web.Controllers
{
    public class NotificationsController : BaseController
    {
        readonly INotificationService _notificationService;
        readonly IUserNotificationService _userNotificationService;
        public NotificationsController(CurrentUser currentUser, INotificationService notificationService, IUserNotificationService userNotificationService) : base(currentUser)
        {
            _notificationService = notificationService;
            _userNotificationService = userNotificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync([FromQuery] NotificationQuery query)
            => Response(await _notificationService.GetListAsync(CurrentUser.NameSpace.Id, CurrentUser.NameIdentifier, query));

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDetailAsync([FromRoute] Guid id)
            => Response(await _notificationService.GetDetailAsync(CurrentUser.NameIdentifier, CurrentUser.NameSpace.Id, id));

        [HttpPost("mark-as-read")]
        public async Task<IActionResult> MarkAsReadAsync([FromBody] Guid notification)
            => Response(await _userNotificationService.MarkAsReadAsync(CurrentUser.NameIdentifier, notification));

    }
}
