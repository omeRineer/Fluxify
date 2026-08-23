using Fluxify.Application.Models.Common;
using Fluxify.Application.Models.Rest.Notification;
using Fluxify.Application.Services.Interfaces.Mongo;
using Fluxify.Application.Services.Mongo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fluxify.Api.Integration.Controllers
{
    public class NotificationsController : BaseController
    {
        readonly INotificationService _notificationService;
        public NotificationsController(CurrentNameSpace currentNameSpace, INotificationService notificationService) : base(currentNameSpace)
        {
            _notificationService = notificationService;
        }

        [HttpPost("fire")]
        public async Task<IActionResult> FireAsync([FromBody] FireNotification.Request request)
            => Response(await _notificationService.FireAsync(CurrentNameSpace.NameIdentifier,
                                                             request));

    }
}
