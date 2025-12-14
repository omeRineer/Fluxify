using Fluxify.Application.Models.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

            await Clients.All.SendAsync("UserConnected", new PushToClientUserConnectedInfoModel
            {
                UserId = Context.UserIdentifier,
                ConnectedDate = DateTime.Now
            });

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            HubConnectionManager.RemoveConnection(Context);

            await Clients.All.SendAsync("UserConnected", new PushToClientUserDisconnectedInfoModel
            {
                UserId = Context.UserIdentifier,
                DisconnectedDate = DateTime.Now
            });

            await base.OnDisconnectedAsync(exception);
        }
    }
}
