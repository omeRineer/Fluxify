using Fluxify.Application.Models.Common;
using Fluxify.Application.Models.Rest.Profile;
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
        readonly IProfileService _profileService;

        public MeController(CurrentUser currentUser, IProfileService profileService) : base(currentUser)
        {
            _profileService = profileService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
            => Response(await _profileService.GetDetailAsync(CurrentUser.NameIdentifier));

        [HttpPost("save")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateProfile.Request req)
            => Response(await _profileService.UpdateAsync(CurrentUser.NameIdentifier, req));


    }
}
