using Fluxify.Application.Models.Common;
using Fluxify.Application.Models.Rest.Chat;
using Fluxify.Application.Models.Rest.Message;
using Fluxify.Application.Services.Interfaces.Core;
using Fluxify.Application.Services.Interfaces.Mongo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Fluxify.Api.Web.Controllers
{
    public class ChatController : BaseController
    {
        readonly IMessageService _messageService;
        readonly IChatService _chatService;
        public ChatController(CurrentUser currentUser, IMessageService messageService, IChatService chatService) : base(currentUser)
        {
            _messageService = messageService;
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateChat.Request req)
            => Response(await _chatService.CreateAsync(CurrentUser.NameSpace.Id, CurrentUser.NameIdentifier, req));

        [HttpGet]
        public async Task<IActionResult> GetListAsync()
            => Response(await _chatService.GetListAsync(CurrentUser.NameSpace.Id, CurrentUser.NameIdentifier));

        [HttpPost("{chatId:guid}/message")]
        public async Task<IActionResult> SendMessageAsync([FromRoute] Guid chatId, [FromBody] SendMessage.Request req)
            => Response(await _messageService.SendAsync(CurrentUser.NameIdentifier, CurrentUser.NameSpace.Id, chatId, req));

        [HttpGet("{chatId:guid}/history")]
        public async Task<IActionResult> GetHistoryAsync([FromRoute] Guid chatId)
            => Response(await _messageService.GetHistoryAsync(CurrentUser.NameIdentifier, chatId));

    }
}
