using Fluxify.Application.Models.Client;
using Fluxify.Application.Models.Rest.Notification;
using Fluxify.Application.Services.Interfaces;
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
        public NotificationsController(CurrentUser currentUser, INotificationService notificationService) : base(currentUser)
        {
            NotificationService = notificationService;
        }

        [HttpPost("Send")]
        public async Task<IActionResult> SendAsync([FromBody] CreateNotificationRequest request)
        {
            var result = await NotificationService.CreateAsync(request);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetListAsync()
        {
            var result = await NotificationService.GetListAsync();

            return Ok(result);
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> GetDetailAsync([FromRoute] Guid id)
        //{
        //    var result = await NotificationService.GetDetailAsync(id);

        //    return Ok(result);
        //}
    }
}
