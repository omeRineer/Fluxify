using Fluxify.Application.Models.Common;
using Fluxify.Application.Models.Rest.Notification;
using Fluxify.Application.Services.Interfaces;
using Fluxify.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace Fluxify.Api.Open.Controllers
{
    public class NotificationsController : BaseController
    {
        readonly INotificationService NotificationService;
        readonly IUserNotificationService UserNotificationService;
        public NotificationsController(CurrentUser currentUser, INotificationService notificationService, IUserNotificationService userNotificationService) : base(currentUser)
        {
            NotificationService = notificationService;
            UserNotificationService = userNotificationService;
        }

        [HttpPost("Send")]
        public async Task<IActionResult> SendAsync([FromBody] CreateNotification.Request request)
        {
            var result = await NotificationService.CreateAsync(CurrentUser.NameSpaceId,
                                                               CurrentUser.NameIdentifier,                                     
                                                               request);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailAsync([FromRoute] Guid id)
        {
            var result = await NotificationService.GetDetailAsync(id, CurrentUser.NameIdentifier);

            return Ok(result);
        }

        [HttpPut("{id}/Read")]
        public async Task<IActionResult> ReadAsync([FromRoute] Guid id)
        {
            var result = await UserNotificationService.MarkAsStatusAsync(NotificationStatus.Read,
                                                                         new[] { id },
                                                                         new[] { CurrentUser.NameIdentifier });

            return Ok(result);
        }
    }
}
