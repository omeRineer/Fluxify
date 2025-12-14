using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Fluxify.Application.Hubs
{
    [Authorize(AuthenticationSchemes = "FluxifySecurityScheme")]
    public class NotificationHub : Hub
    {
        readonly HubConnectionManager<NotificationHub> HubConnectionManager;

        public NotificationHub(HubConnectionManager<NotificationHub> hubConnectionManager)
        {
            HubConnectionManager = hubConnectionManager;
        }

        public override async Task OnConnectedAsync()
        {
            HubConnectionManager.AddConnection(Context);

            foreach (var claim in Context.User.Claims.Where(f => f.Type == "Topic"))
                await Groups.AddToGroupAsync(Context.ConnectionId, claim.Value);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            HubConnectionManager.RemoveConnection(Context);
            await base.OnDisconnectedAsync(exception);
        }
    }
}
