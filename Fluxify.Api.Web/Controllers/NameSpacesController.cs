using Fluxify.Application.Models.Common;
using Fluxify.Application.Models.Rest.NameSpace;
using Fluxify.Application.Services.Interfaces.Core;
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
    }
}
