using Fluxify.Application.Models.Enums;
using Fluxify.Application.Models.SignalR;
using Fluxify.Application.Services.Interfaces;
using Fluxify.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Fluxify.Application.Hubs
{
    [Authorize(AuthenticationSchemes = "FluxifySecurityScheme")]
    public class PresenceHub : Hub
    {
        readonly HubConnectionManager<PresenceHub> HubConnectionManager;
        public PresenceHub(HubConnectionManager<PresenceHub> hubConnectionManager)
        {
            HubConnectionManager = hubConnectionManager;
        }

        public override async Task OnConnectedAsync()
        {
            HubConnectionManager.AddConnection(Context);

            await Clients.All.SendAsync("ReceiveUserState", new PushToClientUserStateInfoModel
            {
                UserId = Context.User.Claims.Single(f => f.Type == FluxifyClaims.Key).Value,
                Date = DateTime.Now,
                IsConnected = true
            });

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            HubConnectionManager.RemoveConnection(Context);

            await Clients.All.SendAsync("ReceiveUserState", new PushToClientUserStateInfoModel
            {
                UserId = Context.User.Claims.Single(f => f.Type == FluxifyClaims.Key).Value,
                Date = DateTime.Now,
                IsConnected = false
            });

            await base.OnDisconnectedAsync(exception);
        }

        public async Task GetOnlineUsers()
        {
            var nameSpace = Context.User.Claims.Single(f => f.Type == FluxifyClaims.NameSpace).Value;

            var onlineSubscribers = HubConnectionManager.GetAllConnections(nameSpace).Select(s => new PushToClientUserStateInfoModel
            {
                UserId = s.Key,
                Date = s.ConnectedDate,
                IsConnected = true
            }).ToList();

            await Clients.Caller.SendAsync("ReceiveOnlineUsers", onlineSubscribers);
        }

        
    }
}
