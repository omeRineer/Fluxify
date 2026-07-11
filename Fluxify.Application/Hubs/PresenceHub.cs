using Fluxify.Application.Models.Enums;
using Fluxify.Application.Models.Hub;
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
            ConnectionUser connectionUser = HubConnectionManager.AddConnection(Context);

            foreach (var topic in Context.User.Claims.Where(f => f.Type == FluxifyClaims.Topic))
                await Groups.AddToGroupAsync(Context.ConnectionId, $"{connectionUser.NameSpace.Id}:{topic.Value}");

            await Groups.AddToGroupAsync(Context.ConnectionId, connectionUser.NameSpace.Signature);

            if (connectionUser.Connections.Count == 1)
            {
                await Clients.Group(connectionUser.NameSpace.Signature).SendAsync(HubEvents.PresenceHub.ReceiveUserStates, new PushToClientUserStateInfoModel
                {
                    UserId = Context.User.Claims.Single(f => f.Type == FluxifyClaims.Key).Value,
                    Date = DateTime.UtcNow,
                    Status = (int) UserAvailabilityStatus.Online
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
                await Clients.Group(connectionUser.NameSpace.Signature).SendAsync(HubEvents.PresenceHub.ReceiveUserStates, new PushToClientUserStateInfoModel
                {
                    UserId = Context.User.Claims.Single(f => f.Type == FluxifyClaims.Key).Value,
                    Date = DateTime.UtcNow,
                    Status = (int) UserAvailabilityStatus.Offline
                });
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task GetOnlineUsers()
        {
            ConnectionUser connectionUser = HubConnectionManager.GetConnection(Context.User.Claims.Single(f => f.Type == ClaimTypes.NameIdentifier).Value);

            var onlineSubscribers = HubConnectionManager.GetAllConnections(connectionUser.NameSpace.Id).Select(s => new PushToClientUserStateInfoModel
            {
                UserId = s.Key,
                Date = s.ConnectedDate,
                Status = s.Status.Id,
                Title = s.Status.Name
            }).ToList();

            await Clients.Caller.SendAsync(HubEvents.PresenceHub.ReceiveOnlineUsers, onlineSubscribers);
        }

        public async Task SetAvailabilityStatus(int status)
        {
            ConnectionUser connectionUser = HubConnectionManager.GetConnection(Context.User.Claims.Single(f => f.Type == ClaimTypes.NameIdentifier).Value);

            connectionUser.Status.Id = status;
            await Clients.Group(connectionUser.NameSpace.Signature).SendAsync(HubEvents.PresenceHub.ReceiveUserStates, new PushToClientUserStateInfoModel
            {
                UserId = Context.User.Claims.Single(f => f.Type == FluxifyClaims.Key).Value,
                Status = status,
                Title = connectionUser.Status.Name,
                Date = DateTime.UtcNow
            });
        }
    }
}
