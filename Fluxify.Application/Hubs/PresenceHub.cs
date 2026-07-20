using Fluxify.Application.Models.Enums;
using Fluxify.Application.Models.Hub;
using Fluxify.Application.Models.SignalR;
using Fluxify.Application.Services.Interfaces;
using Fluxify.Application.Utilities.Helpers;
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
                await Clients.Group(connectionUser.NameSpace.Signature).SendAsync(HubEvents.PresenceHub.ReceiveUserStates, new UserStateInfoMessage
                {
                    UserId = Context.User.Claims.Single(f => f.Type == FluxifyClaims.Key).Value,
                    Status = EnumHelper.ToEnumString<UserAvailabilityStatus>((int)UserAvailabilityStatus.Online),
                    Date = DateTime.UtcNow
                });

            }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            ConnectionUser connectionUser = HubConnectionManager.GetConnection(Context.User.Claims.Single(f => f.Type == ClaimTypes.NameIdentifier).Value);
            int connectionCount = HubConnectionManager.RemoveConnection(Context);

            if (connectionCount == 0)
            {
                await Clients.Group(connectionUser.NameSpace.Signature).SendAsync(HubEvents.PresenceHub.ReceiveUserStates, new UserStateInfoMessage
                {
                    UserId = Context.User.Claims.Single(f => f.Type == FluxifyClaims.Key).Value,
                    Date = DateTime.UtcNow,
                    Status = EnumHelper.ToEnumString<UserAvailabilityStatus>((int)UserAvailabilityStatus.Offline)
                });
            }

            await base.OnDisconnectedAsync(exception);
        }

        public async Task GetOnlineUsers()
        {
            ConnectionUser connectionUser = HubConnectionManager.GetConnection(Context.User.Claims.Single(f => f.Type == ClaimTypes.NameIdentifier).Value);

            var onlineSubscribers = HubConnectionManager.GetAllConnections(connectionUser.NameSpace.Id).Select(s => new UserStateInfoMessage
            {
                UserId = s.Key,
                Date = s.ConnectedDate,
                Status = s.Status
            }).ToList();

            await Clients.Caller.SendAsync(HubEvents.PresenceHub.ReceiveOnlineUsers, onlineSubscribers);
        }

        public async Task SetAvailabilityStatus(string status)
        {
            ConnectionUser connectionUser = HubConnectionManager.GetConnection(Context.User.Claims.Single(f => f.Type == ClaimTypes.NameIdentifier).Value);

            if (EnumHelper.HasValue<UserAvailabilityStatus>(status))
            {
                connectionUser.SetStatus(status);
                await Clients.Group(connectionUser.NameSpace.Signature).SendAsync(HubEvents.PresenceHub.ReceiveUserStates, new UserStateInfoMessage
                {
                    UserId = connectionUser.Key,
                    Status = status,
                    Date = DateTime.UtcNow
                });
            }
        }
    }
}
