using Fluxify.Application.Models.Common;
using Fluxify.Application.Models.Rest.UserDevice;
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
        readonly IUserDeviceService UserDeviceService;
        public MeController(CurrentUser currentUser, IUserService UserService, INotificationService notificationService, IUserDeviceService userDeviceService) : base(currentUser)
        {
            this.UserService = UserService;
            NotificationService = notificationService;
            UserDeviceService = userDeviceService;
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

        //[HttpGet("Devices")]
        //public async Task<IActionResult> GetDevicesAsync()
        //    => Ok(await UserDeviceService.SetAsync(CurrentUser.NameIdentifier,
        //                                           req));

        [HttpPost("Device/Set")]
        public async Task<IActionResult> SetDeviceAsync(SetUserDevice.Request req)
            => Ok(await UserDeviceService.SetAsync(CurrentUser.NameIdentifier,
                                                   req));
    }
}
