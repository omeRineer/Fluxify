using Fluxify.Application.Models.Enums;
using Fluxify.Application.Models.Hub;
using Fluxify.Application.Models.SignalR;
using Fluxify.Application.Services.Interfaces;
using Fluxify.Application.Services.Interfaces.Core;
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
        readonly HubConnectionManager<PresenceHubUser> HubConnectionManager;
        readonly IUserTopicService _userTopicService;
        public PresenceHub(HubConnectionManager<PresenceHubUser> hubConnectionManager, IUserTopicService userTopicService)
        {
            HubConnectionManager = hubConnectionManager;
            _userTopicService = userTopicService;
        }

        public override async Task OnConnectedAsync()
        {
            // TODO : Race condition riski
            PresenceHubUser hubUser = HubConnectionManager.AddConnection(Context);
            if (hubUser.Topics == null)
                hubUser.Topics = await _userTopicService.GetTopicsByUser(hubUser.Id);

            foreach (var topic in hubUser.Topics)
                await Groups.AddToGroupAsync(Context.ConnectionId, $"{hubUser.NameSpace.Id}:{topic}");

            await Groups.AddToGroupAsync(Context.ConnectionId, hubUser.NameSpace.Signature);

            if (hubUser.Connections.Count == 1)
            {
                await Clients.Group(hubUser.NameSpace.Signature).SendAsync(HubEvents.PresenceHub.ReceiveUserStates, new UserStateInfoMessage
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
            HubUser connectionUser = HubConnectionManager.GetConnection(Guid.Parse(Context.User.Claims.Single(f => f.Type == ClaimTypes.NameIdentifier).Value));
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

        public async Task SetAvailabilityStatus(string status)
        {
            PresenceHubUser connectionUser = HubConnectionManager.GetConnection(Guid.Parse(Context.User.Claims.Single(f => f.Type == ClaimTypes.NameIdentifier).Value));

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
