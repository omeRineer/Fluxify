using Fluxify.Application.Models.Client;
using Fluxify.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace Fluxify.Api.Web.Controllers
{
    public class NotificationsController : BaseController
    {
        readonly INotificationService NotificationService;
        public NotificationsController(CurrentUser currentUser, INotificationService notificationService) : base(currentUser)
        {
            NotificationService = notificationService;
        }
    }
}
