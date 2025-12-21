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

            await Clients.All.SendAsync("UserState", new PushToClientUserStateInfoModel
            {
                UserId = Context.UserIdentifier,
                Date = DateTime.Now,
                IsConnected = true
            });

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            HubConnectionManager.RemoveConnection(Context);

            await Clients.All.SendAsync("UserState", new PushToClientUserStateInfoModel
            {
                UserId = Context.UserIdentifier,
                Date = DateTime.Now,
                IsConnected = false
            });

            await base.OnDisconnectedAsync(exception);
        }

        public async Task<List<PushToClientUserStateInfoModel>> GetOnlineSubscribers()
        {
            var nameSpace = Context.User.Claims.Single(f => f.Type == "NameSpace")?.Value;

            return await Task.FromResult(HubConnectionManager.GetAllConnections(nameSpace).Select(s => new PushToClientUserStateInfoModel
            {
                UserId = s.Id,
                Date = s.ConnectedDate,
                IsConnected = true
            }).ToList());
        }
    }
}
