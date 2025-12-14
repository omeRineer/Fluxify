using Fluxify.Application.Models.Client;
using Fluxify.Application.Models.Rest.NameSpace;
using Fluxify.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fluxify.Api.Web.Controllers
{
    public class NameSpacesController : BaseController
    {
        readonly INameSpaceService NameSpaceService;

        public NameSpacesController(CurrentUser currentUser, INameSpaceService nameSpaceService) : base(currentUser)
        {
            NameSpaceService = nameSpaceService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailAsync([FromRoute] Guid id)
        {
            var result = await NameSpaceService.GetDetailAsync(id);

            return Response(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateNameSpaceRequest model)
        {
            var result = await NameSpaceService.CreateAsync(model);

            return Response(result);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateAsync([FromBody] UpdateNameSpaceRequest model)
        {
            var result = await NameSpaceService.UpdateAsync(model);

            return Response(result);
        }
    }
}
