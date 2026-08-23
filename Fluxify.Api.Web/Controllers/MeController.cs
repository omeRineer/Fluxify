using Fluxify.Application.Models.Common;
using Fluxify.Application.Services.Core;
using Fluxify.Application.Services.Interfaces.Core;
using Fluxify.Application.Services.Interfaces.Mongo;
using Fluxify.Application.Services.Mongo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fluxify.Api.Web.Controllers
{
    public class MeController : BaseController
    {
        readonly IChatService _chatService;
        readonly IUserService _userService;
        readonly INotificationService _notificationService;
        public MeController(CurrentUser currentUser, IChatService chatService, IUserService userService, INotificationService notificationService) : base(currentUser)
        {
            _chatService = chatService;
            _userService = userService;
            _notificationService = notificationService;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfileAsync()
            => Response(await _userService.GetDetailAsync(CurrentUser.NameIdentifier));

    }
}
