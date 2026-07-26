using Fluxify.Application.Models.Enums;
using Fluxify.Application.Models.Hub;
using Fluxify.Application.Models.SignalR;
using Fluxify.Application.Services.Interfaces.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Hubs
{
    [Authorize(AuthenticationSchemes = "FluxifySecurityScheme")]
    public class ChatHub : Hub
    {
        readonly HubConnectionManager<ChatHubUser> HubConnectionManager;
        readonly IChatUserService _chatUserService;
        public ChatHub(HubConnectionManager<ChatHubUser> hubConnectionManager, IChatUserService chatUserService)
        {
            HubConnectionManager = hubConnectionManager;
            _chatUserService = chatUserService;
        }

        public override async Task OnConnectedAsync()
        {
            ChatHubUser chatHubUser = HubConnectionManager.AddConnection(Context);
            if (chatHubUser.Chats == null)
                chatHubUser.Chats = await _chatUserService.GetChatsByUser(chatHubUser.Id);

            foreach (var chat in chatHubUser.Chats)
                await Groups.AddToGroupAsync(Context.ConnectionId, $"chat:{chat}");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            HubConnectionManager.RemoveConnection(Context);

            await base.OnDisconnectedAsync(exception);
        }

        public async Task OnTyping(Guid chatId, bool isTyping)
        {
            var userConnections = HubConnectionManager.GetConnection(Guid.Parse(Context.User.Claims.Single(f => f.Type == ClaimTypes.NameIdentifier).Value)).Connections.Keys.AsEnumerable();
            await Clients.GroupExcept($"chat:{chatId}", userConnections).SendAsync("OnTyping", new
            {
                chatId,
                isTyping
            });
        }
    }
}
