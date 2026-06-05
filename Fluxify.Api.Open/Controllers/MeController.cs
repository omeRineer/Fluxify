using Fluxify.Application.Models.Common;
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
        readonly INotificationService NotificationService;
        public MeController(CurrentUser currentUser, IUserService UserService, INotificationService notificationService) : base(currentUser)
        {
            this.UserService = UserService;
            NotificationService = notificationService;
        }

        [HttpGet("Profile")]
        public async Task<IActionResult> GetProfileAsync()
            => Ok(await UserService.GetDetailAsync(CurrentUser.NameIdentifier));

        [HttpGet("Notifications")]
        public async Task<IActionResult> GetNotificationsAsync()
            => Ok(await NotificationService.GetListAsync(CurrentUser.NameIdentifier,
                                                                new[]
                                                                {
                                                                    NotificationStatus.Delivered,
                                                                    NotificationStatus.Read
                                                                }));
    }
}
