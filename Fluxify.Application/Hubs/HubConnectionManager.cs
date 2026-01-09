using Fluxify.Application.Models;
using Fluxify.Application.Models.Enums;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Hubs
{
    public class HubConnectionManager<THub>
    {
        Dictionary<string, ConnectionUser> Connections { get; }
        public HubConnectionManager()
        {
            Connections = new();
        }

        public ConnectionUser GetConnection(string userId)
        {
            lock (Connections)
            {
                return Connections.Values.Single(f => f.Id == Guid.Parse(userId));
            }
        }
        public List<ConnectionUser> GetAllConnections(string nameSpace)
        {
            lock (Connections)
            {
                return Connections.Values.Where(f => f.NameSpace == nameSpace).ToList();
            }
        }

        public int AddConnection(HubCallerContext context)
        {
            lock (Connections)
            {
                if (!Connections.TryGetValue(context.UserIdentifier, out ConnectionUser user))
                {
                    user = new ConnectionUser
                    {
                        Id = Guid.Parse(context.User.Claims.Single(f => f.Type == ClaimTypes.NameIdentifier).Value),
                        Key = context.User.Claims.Single(f => f.Type == FluxifyClaims.Key).Value,
                        NameSpaceId = Guid.Parse(context.User.Claims.Single(f => f.Type == FluxifyClaims.NameSpaceId).Value),
                        NameSpace = context.User.Claims.Single(f => f.Type == FluxifyClaims.NameSpace).Value,
                        ConnectedDate = DateTime.UtcNow,
                        Topics = context.User.Claims
                            .Where(f => f.Type == "Topic")
                            .Select(s => s.Value)
                            .ToList(),
                        AvailabilityStatus = (int) UserAvailabilityStatus.Online
                    };

                    Connections[context.UserIdentifier] = user;
                }

                user.Connections.Add(context.ConnectionId);

                return user.Connections.Count;
            }
        }

        public int RemoveConnection(HubCallerContext context)
        {
            lock (Connections)
            {
                if (!Connections.TryGetValue(context.UserIdentifier, out var user))
                    return 0;

                user.Connections.Remove(context.ConnectionId);

                if (user.Connections.Count == 0)
                {
                    Connections.Remove(context.UserIdentifier);
                    return 0;
                }

                return user.Connections.Count;
            }
        }

    }
}
