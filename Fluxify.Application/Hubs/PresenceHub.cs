using Fluxify.Application.Models.Enums;
using Fluxify.Application.Models.Hub;
using Fluxify.Application.Models.Rest.UserDevice;
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
    [Authorize]
    public class PresenceHub : Hub
    {
        readonly HubConnectionManager<PresenceHubUser> HubConnectionManager;
        readonly IUserTopicService _userTopicService;
        readonly IUserDeviceService _userDeviceService;
        public PresenceHub(HubConnectionManager<PresenceHubUser> hubConnectionManager, IUserTopicService userTopicService, IUserDeviceService userDeviceService)
        {
            HubConnectionManager = hubConnectionManager;
            _userTopicService = userTopicService;
            _userDeviceService = userDeviceService;
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
                await Clients.Group(hubUser.NameSpace.Signature).SendAsync(HubEvents.PresenceHub.ReceiveUserStates, new UserStateInfoPayload
                {
                    Key = Context.User.Claims.Single(f => f.Type == FluxifyClaims.Key).Value,
                    Status = EnumHelper.ToEnumString<UserAvailabilityStatus>((int)UserAvailabilityStatus.Online),
                    Date = DateTime.UtcNow
                });

            }

            var httpContext = Context.GetHttpContext();
            var platform = httpContext?.Request?.Query[FluxifyHeaders.Device.Platform].FirstOrDefault();
            await _userDeviceService.RegisterAsync(hubUser.Id, hubUser.NameSpace.Id, new SetUserDevice
            {
                DeviceId = httpContext?.Request?.Query[FluxifyHeaders.Device.DeviceId],
                Platform = string.IsNullOrWhiteSpace(platform)
                    ? EnumHelper.ToEnumString<DeviceType>((int)DeviceType.Web)
                    : platform,
                Token = httpContext?.Request?.Query[FluxifyHeaders.Device.Token].FirstOrDefault()
            });

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            PresenceHubUser connectionUser = HubConnectionManager.GetConnection(Guid.Parse(Context.User.Claims.Single(f => f.Type == ClaimTypes.NameIdentifier).Value));
            int connectionCount = HubConnectionManager.RemoveConnection(Context);

            if (connectionCount == 0)
            {
                await Clients.Group(connectionUser.NameSpace.Signature).SendAsync(HubEvents.PresenceHub.ReceiveUserStates, new UserStateInfoPayload
                {
                    Key = Context.User.Claims.Single(f => f.Type == FluxifyClaims.Key).Value,
                    Date = DateTime.UtcNow,
                    Status = EnumHelper.ToEnumString<UserAvailabilityStatus>((int)UserAvailabilityStatus.Offline)
                });
            }

            await base.OnDisconnectedAsync(exception);
        }



        #region Events
        public async Task SetAvailabilityStatus(string status)
        {
            PresenceHubUser connectionUser = HubConnectionManager.GetConnection(Guid.Parse(Context.User.Claims.Single(f => f.Type == ClaimTypes.NameIdentifier).Value));

            if (EnumHelper.HasValue<UserAvailabilityStatus>(status))
            {
                connectionUser.SetStatus(status);
                await Clients.Group(connectionUser.NameSpace.Signature).SendAsync(HubEvents.PresenceHub.ReceiveUserStates, new UserStateInfoPayload
                {
                    Key = connectionUser.Key,
                    Status = status,
                    Date = DateTime.UtcNow
                });
            }
        }
        #endregion

    }
}
