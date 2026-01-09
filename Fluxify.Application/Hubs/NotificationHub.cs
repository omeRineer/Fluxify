using Fluxify.Application.Models;
using Fluxify.Application.Models.Enums;
using Fluxify.Application.Models.SignalR;
using Fluxify.Application.Services;
using Fluxify.Application.Services.Interfaces;
using Fluxify.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Fluxify.Application.Hubs
{
    [Authorize(AuthenticationSchemes = "FluxifySecurityScheme")]
    public class NotificationHub : Hub
    {
        readonly INotificationService NotificationService;
        readonly HubConnectionManager<NotificationHub> HubConnectionManager;

        public NotificationHub(HubConnectionManager<NotificationHub> hubConnectionManager, INotificationService notificationService)
        {
            HubConnectionManager = hubConnectionManager;
            this.NotificationService = notificationService;
        }

        public override async Task OnConnectedAsync()
        {
            HubConnectionManager.AddConnection(Context);
            ConnectionUser connectionUser = HubConnectionManager.GetConnection(Context.User.Claims.Single(f => f.Type == ClaimTypes.NameIdentifier).Value);

            foreach (var claim in Context.User.Claims.Where(f => f.Type == "Topic"))
                await Groups.AddToGroupAsync(Context.ConnectionId, $"{connectionUser.NameSpace}:{claim.Value}");

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            HubConnectionManager.RemoveConnection(Context);
            await base.OnDisconnectedAsync(exception);
        }



        public async Task GetPendingNotifications()
        {
            ConnectionUser connectionUser = HubConnectionManager.GetConnection(Context.User.Claims.Single(f => f.Type == ClaimTypes.NameIdentifier).Value);
            var pendingNotifications = await NotificationService.GetAndMarkPendingAsync(connectionUser.Id, connectionUser.NameSpaceId);

            var sendingNotifications = pendingNotifications.Data?.Notifications?.Select(s => new PushToClientNotificationModel
            {
                Id = s.Id,
                ContentType = s.ContentType,
                CreateDate = s.CreateDate,
                Custom = s.Properties,
                Status = s.Status,
                Level = s.Level,
                Sender = s.Sender,
                Title = s.Title
            }).ToList();

            await Clients.Caller.SendAsync("ReceivePendingNotifications", sendingNotifications);
        }
    }
}
