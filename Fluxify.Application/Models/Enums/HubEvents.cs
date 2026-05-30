using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Models.Enums
{
    public static class HubEvents
    {
        public static class PresenceHub
        {
            public const string ReceiveUserStates = "ReceiveUserStates";
            public const string ReceiveOnlineUsers = "ReceiveOnlineUsers";
        }

        public static class NotificationHub
        {
            public const string ReceivePendingNotifications = "ReceivePendingNotifications";
            public const string ReceiveNotification = "ReceiveNotification";
        }
    }
}
