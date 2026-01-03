using Fluxify.Application.Models.SignalR;
using Fluxify.Application.Services;
using Fluxify.Application.Services.Interfaces;
using Fluxify.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

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

            foreach (var claim in Context.User.Claims.Where(f => f.Type == "Topic"))
                await Groups.AddToGroupAsync(Context.ConnectionId, claim.Value);

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            HubConnectionManager.RemoveConnection(Context);
            await base.OnDisconnectedAsync(exception);
        }



        public async Task GetPendingNotifications()
        {
            var userId = Context.UserIdentifier;
            var pendingNotifications = await NotificationService.GetListByStatusAsync((int)NotificationStatus.Pending);

            if (pendingNotifications.Data == null || pendingNotifications.Data.Notifications?.Any() != true)
                return;

            await NotificationService.MarkAsStatusByNotificationAsync(NotificationStatus.Delivered, Guid.Parse(userId), pendingNotifications.Data.Notifications.Select(s => s.Id).ToArray());

            var sendingNotifications = pendingNotifications.Data?.Notifications?.Select(s => new PushToClientNotificationModel
            {
                Id = s.Id,
                ContentType = s.ContentType,
                CreateDate = s.CreateDate,
                Custom = s.Properties,
                Status = (int)NotificationStatus.Delivered,
                Level = s.Level,
                Sender = s.Sender,
                Title = s.Title
            }).ToList();

            await Clients.Caller.SendAsync("ReceivePendingNotifications", sendingNotifications);
        }
    }
}
