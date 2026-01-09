using Fluxify.Application.Models;
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
            int connectionCount = HubConnectionManager.AddConnection(Context);
            ConnectionUser connectionUser = HubConnectionManager.GetConnection(Context.User.Claims.Single(f => f.Type == ClaimTypes.NameIdentifier).Value);

            await Groups.AddToGroupAsync(Context.ConnectionId, connectionUser.NameSpace);

            if (connectionCount == 1)
            {
                await Clients.Group(connectionUser.NameSpace).SendAsync("ReceiveUserState", new PushToClientUserStateInfoModel
                {
                    UserId = Context.User.Claims.Single(f => f.Type == FluxifyClaims.Key).Value,
                    Date = DateTime.UtcNow,
                    AvailabilityStatus = (int) UserAvailabilityStatus.Online
                });
                
            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            ConnectionUser connectionUser = HubConnectionManager.GetConnection(Context.User.Claims.Single(f => f.Type == ClaimTypes.NameIdentifier).Value);
            int connectionCount = HubConnectionManager.RemoveConnection(Context);

            if(connectionCount == 0)
            {
                await Clients.Group(connectionUser.NameSpace).SendAsync("ReceiveUserState", new PushToClientUserStateInfoModel
                {
                    UserId = Context.User.Claims.Single(f => f.Type == FluxifyClaims.Key).Value,
                    Date = DateTime.UtcNow,
                    AvailabilityStatus = (int) UserAvailabilityStatus.Offline
                });
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task GetOnlineUsers()
        {
            ConnectionUser connectionUser = HubConnectionManager.GetConnection(Context.User.Claims.Single(f => f.Type == ClaimTypes.NameIdentifier).Value);

            var onlineSubscribers = HubConnectionManager.GetAllConnections(connectionUser.NameSpace).Select(s => new PushToClientUserStateInfoModel
            {
                UserId = s.Key,
                Date = s.ConnectedDate,
                AvailabilityStatus = s.AvailabilityStatus
            }).ToList();

            await Clients.Caller.SendAsync("ReceiveOnlineUsers", onlineSubscribers);
        }


    }
}
