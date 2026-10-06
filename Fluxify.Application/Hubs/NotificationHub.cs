using Fluxify.Application.Models.Hub;
using Fluxify.Application.Services.Interfaces.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Hubs
{
    [Authorize]
    public class NotificationHub :  Hub
    {
        readonly HubConnectionManager<NotificationHubUser> HubConnectionManager;
        readonly IUserTopicService _userTopicService;

        public NotificationHub(HubConnectionManager<NotificationHubUser> hubConnectionManager, IUserTopicService userTopicService)
        {
            HubConnectionManager = hubConnectionManager;
            _userTopicService = userTopicService;
        }

        public override async Task OnConnectedAsync()
        {
            NotificationHubUser notificationHubUser = HubConnectionManager.AddConnection(Context);

            if (notificationHubUser.Topics == null)
                notificationHubUser.Topics = await _userTopicService.GetTopicsByUser(notificationHubUser.Id);

            foreach (var topic in notificationHubUser.Topics)
                await Groups.AddToGroupAsync(Context.ConnectionId, $"{notificationHubUser.NameSpace.Id}:{topic}");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            HubConnectionManager.RemoveConnection(Context);

            await base.OnDisconnectedAsync(exception);
        }
    }
}
