using Fluxify.Application.Models.Common;
using Fluxify.Application.Services.Core;
using Fluxify.Application.Services.Interfaces.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fluxify.Api.Open.Controllers
{
    public class DevicesController : BaseController
    {
        readonly IUserDeviceService UserDeviceService;
        public DevicesController(CurrentUser currentUser, IUserDeviceService userDeviceService) : base(currentUser)
        {
            UserDeviceService = userDeviceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDevicesAsync()
            => Response(await UserDeviceService.GetListAsync(CurrentUser.NameIdentifier));
    }
}
