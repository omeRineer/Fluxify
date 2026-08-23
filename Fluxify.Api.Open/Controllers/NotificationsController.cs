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

        [HttpGet]
        public async Task<IActionResult> GetNotificationsAsync()
            => Response(await NotificationService.GetListAsync(CurrentUser.NameSpace.Id, CurrentUser.NameIdentifier));


        // TODO : Owner check
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailAsync([FromRoute] Guid id)
            => Response(await NotificationService.GetDetailAsync(CurrentUser.NameSpace.Id, 
                                                                 CurrentUser.NameIdentifier, 
                                                                 id));

        [HttpPut("{id}/read")]
        public async Task<IActionResult> ReadAsync([FromRoute] Guid id)
            => Response(await UserNotificationService.AddAsync(CurrentUser.NameIdentifier, id));
    }
}
