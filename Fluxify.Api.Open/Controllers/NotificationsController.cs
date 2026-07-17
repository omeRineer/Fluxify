using Fluxify.Application.Models.Common;
using Fluxify.Application.Models.Rest.Notification;
using Fluxify.Application.Services.Interfaces.Mongo;
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

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateNotification.Request request)
        {
            var result = await NotificationService.CreateAsync(CurrentUser.NameSpace.Id,
                                                               CurrentUser.NameIdentifier,                                     
                                                               request);

            return Response(result);
        }

        [HttpPost("Fire")]
        public async Task<IActionResult> FireAsync([FromBody] FireNotification.Request request)
        {
            var result = await NotificationService.FireAsync(CurrentUser.NameSpace.Id,
                                                             request);

            return Response(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailAsync([FromRoute] Guid id)
        {
            var result = await NotificationService.GetDetailAsync(id, CurrentUser.NameIdentifier);

            return Response(result);
        }

        [HttpPut("{id}/Read")]
        public async Task<IActionResult> ReadAsync([FromRoute] Guid id)
        {
            var result = await UserNotificationService.AddAsync(CurrentUser.NameIdentifier, id);

            return Response(result);
        }
    }
}
