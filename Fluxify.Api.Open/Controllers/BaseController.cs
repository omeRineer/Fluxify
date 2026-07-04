using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fluxify.Application.Utilities.ApiResponse;
using R = Fluxify.Application.Utilities.Result;
using Microsoft.AspNetCore.Authorization;
using Fluxify.Application.Models.Common;

namespace Fluxify.Api.Open.Controllers
{
    [Route("openapi/[controller]")]
    [Authorize(AuthenticationSchemes = "FluxifySecurityScheme")]
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
