using Fluxify.Application.Models.Common;
using Fluxify.Application.Services.Interfaces.Mongo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fluxify.Api.Web.Controllers
{
    public class NotificationsController : BaseController
    {
        readonly INotificationService _notificationService;
        public NotificationsController(CurrentUser currentUser, INotificationService notificationService) : base(currentUser)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetNotificationsAsync()
            => Response(await _notificationService.GetListAsync(CurrentUser.NameSpace.Id, CurrentUser.NameIdentifier));

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDetailAsync([FromRoute] Guid id)
            => Response(await _notificationService.GetDetailAsync(CurrentUser.NameIdentifier, CurrentUser.NameSpace.Id, id));

    }
}
