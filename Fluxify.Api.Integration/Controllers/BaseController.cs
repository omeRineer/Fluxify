using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Fluxify.Application.Utilities.ApiResponse;
using R = Fluxify.Application.Utilities.Result;
using Microsoft.AspNetCore.Authorization;
using Fluxify.Application.Models.Common;

namespace Fluxify.Api.Integration.Controllers
{
    [Route("integrationapi/[controller]")]
    [Authorize(AuthenticationSchemes = "FluxifySecurityScheme")]
    public class BaseController : ControllerBase
    {
        protected readonly CurrentNameSpace CurrentNameSpace;
        public BaseController(CurrentNameSpace currentNameSpace)
        {
            CurrentNameSpace = currentNameSpace;
        }

        protected IActionResult Response(R.IResult result)
            => result.Success ? Ok(result) : BadRequest(result);
    }
}
