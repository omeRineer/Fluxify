using Fluxify.Application.Models.Common;
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
        public async Task<IActionResult> GetNotificationsAsync()
            => Response(await _notificationService.GetListAsync(CurrentUser.NameSpace.Id, CurrentUser.NameIdentifier));

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDetailAsync([FromRoute] Guid id)
            => Response(await _notificationService.GetDetailAsync(CurrentUser.NameIdentifier, CurrentUser.NameSpace.Id, id));

        [HttpPost("mark-as-read")]
        public async Task<IActionResult> MarkAsReadAsync([FromBody] Guid[] notifications)
            => Response(await _userNotificationService.AddAsync(CurrentUser.NameIdentifier, notifications));

    }
}
