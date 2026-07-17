using Fluxify.Application.Models.Common;
using Fluxify.Application.Models.Rest.Message;
using Fluxify.Application.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fluxify.Api.Open.Controllers
{
    public class ChatController : BaseController
    {
        readonly IMessageService _messageService;
        public ChatController(CurrentUser currentUser, IMessageService messageService) : base(currentUser)
        {
            _messageService = messageService;
        }

        [HttpPost("{chatId:guid}/message")]
        public async Task<IActionResult> SendMessageAsync([FromRoute] Guid chatId, [FromBody] SendMessage.Request req)
            => Response(await _messageService.SendAsync(CurrentUser.NameIdentifier, chatId, CurrentUser.NameSpace.Id, req));
    }
}
