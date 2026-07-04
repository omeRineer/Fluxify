using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fluxify.Application.Utilities.ApiResponse;
using Microsoft.AspNetCore.Authorization;
using Fluxify.Application.Models.Common;

namespace Fluxify.Api.Web.Controllers
{
    [Route("webapi/[controller]")]
    public class BaseController : ControllerBase
    {
        readonly CurrentUser CurrentUser;
        public BaseController(CurrentUser currentUser)
        {
            CurrentUser = currentUser;
        }

        protected IActionResult Response(ApiResponse response)
            => response.Success ? Ok(response) : BadRequest(response);
    }
}
