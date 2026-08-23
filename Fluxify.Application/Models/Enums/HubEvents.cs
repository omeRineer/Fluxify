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
            public const string ReceiveUserStatus = "ReceiveUserStatus";
            public const string ReceiveUserStatusList = "ReceiveUserStatusList";
            public const string ReceivePendingNotifications = "ReceivePendingNotifications";
            public const string ReceiveNotification = "ReceiveNotification";
        }

        public static class ChatHub
        {
            public const string ReceiveMessage = "ReceiveMessage";
        }
    }
}
