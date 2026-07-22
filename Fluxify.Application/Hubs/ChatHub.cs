using Fluxify.Application.Models.Enums;
using Fluxify.Application.Models.Hub;
using Fluxify.Application.Models.SignalR;
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
        readonly HubConnectionManager<ChatHub> HubConnectionManager;
        public ChatHub(HubConnectionManager<ChatHub> hubConnectionManager)
        {
            HubConnectionManager = hubConnectionManager;
        }

        public override async Task OnConnectedAsync()
        {
            HubConnectionManager.AddConnection(Context);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            HubConnectionManager.RemoveConnection(Context);

            await base.OnDisconnectedAsync(exception);
        }

        //public async Task OnTyping(Guid chatId, bool isTyping)
        //    => Clients.
    }
}
