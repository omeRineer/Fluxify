using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fluxify.Application.Utilities.ApiResponse;
using Microsoft.AspNetCore.Authorization;
using Fluxify.Application.Models.Common;
using R = Fluxify.Application.Utilities.Result;

namespace Fluxify.Api.Web.Controllers
{
    [Route("webapi/[controller]")]
    [Authorize]
    public class BaseController : ControllerBase
    {
        protected readonly CurrentUser CurrentUser;
        public BaseController(CurrentUser currentUser)
        {
            CurrentUser = currentUser;
        }

        protected IActionResult Response(R.IResult result)
            => result.Success ? Ok(result) : BadRequest(result);
    }
}
