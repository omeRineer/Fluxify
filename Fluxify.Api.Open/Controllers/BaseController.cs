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

        //protected IActionResult Response(R.IResult result)
        //{
        //    switch (result)
        //    {
        //        case R.IDataResult<dynamic> dataResult:
        //            if (dataResult.Success)
        //                return Ok(new ApiDataResponse<(dataResult.Success, );
        //            else
        //                return BadRequest(new ErrorApiResponse(message: dataResult.Message));
        //        default:
        //            break;
        //    }
        //}
    }
}
