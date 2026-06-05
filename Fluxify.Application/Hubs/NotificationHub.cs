using Fluxify.Application.Models.Enums;
using Fluxify.Application.Models.Hub;
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
        readonly IUserNotificationService UserNotificationService;
        readonly HubConnectionManager<NotificationHub> HubConnectionManager;

        public NotificationHub(HubConnectionManager<NotificationHub> hubConnectionManager, INotificationService notificationService, IUserNotificationService userNotificationService)
        {
            HubConnectionManager = hubConnectionManager;
            NotificationService = notificationService;
            UserNotificationService = userNotificationService;
        }

        public override async Task OnConnectedAsync()
        {
            ConnectionUser connectionUser = HubConnectionManager.AddConnection(Context);

            foreach (var topic in Context.User.Claims.Where(f => f.Type == FluxifyClaims.Topic))
                await Groups.AddToGroupAsync(Context.ConnectionId, $"{connectionUser.NameSpace.Name}:{topic.Value}");

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
            var pendingNotifications = await NotificationService.GetListAsync(connectionUser.Id, new[]
            {
                NotificationStatus.Pending
            });

            if(pendingNotifications.Data.Count > 0)
            {
                var sendingNotifications = pendingNotifications.Data.Items.Select(s => new PushToClientNotificationModel
                {
                    Id = s.Id,
                    ContentType = s.ContentType,
                    CreateDate = s.CreateDate,
                    Properties = s.Properties,
                    Status = s.Status,
                    Level = s.Level,
                    Sender = s.Sender,
                    Title = s.Title
                }).ToList();

                await Clients.Caller.SendAsync(HubEvents.NotificationHub.ReceivePendingNotifications, sendingNotifications);

                await UserNotificationService.MarkAsStatusAsync(NotificationStatus.Delivered, 
                                                                pendingNotifications.Data.Items.Select(s => s.Id).ToArray(), 
                                                                new[] { connectionUser.Id });

            }

        }


    }
}
