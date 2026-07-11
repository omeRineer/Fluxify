using Fluxify.Application.Models.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fluxify.Api.Open.Controllers
{
    public class MessagesController : BaseController
    {
        public MessagesController(CurrentUser currentUser) : base(currentUser)
        {
        }

    }
}
