using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fluxify.Application.Utilities.ApiResponse;
using Fluxify.Application.Models.Client;
using Microsoft.AspNetCore.Authorization;

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

        protected IActionResult Response(ApiResponse response)
            => response.Success ? Ok(response) : BadRequest(response);
    }
}
