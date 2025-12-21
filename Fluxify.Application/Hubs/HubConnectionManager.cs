using Fluxify.Application.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public List<ConnectionUser> GetAllConnections(string nameSpace)
        {
            lock (Connections)
            {
                return Connections.Values.Where(f => f.NameSpace == nameSpace).ToList();
            }
        }

        public void AddConnection(HubCallerContext context)
        {
            lock (Connections)
            {
                if (!Connections.ContainsKey(context.UserIdentifier))
                    Connections[context.UserIdentifier] = new ConnectionUser
                    {
                        ConnectionId = context.ConnectionId,
                        Id = context.UserIdentifier,
                        ConnectedDate = DateTime.Now,
                        NameSpace = context.User.Claims.Single(f => f.Type == "NameSpace").Value,
                        Topics = context.User.Claims.Where(f => f.Type == "Topic")
                                                    .Select(s => s.Value)
                                                    .ToList()
                    };
            }
        }

        public void RemoveConnection(HubCallerContext context)
        {
            lock (Connections)
            {
                if (Connections.ContainsKey(context.UserIdentifier))
                    Connections.Remove(context.UserIdentifier);
            }
        }

    }
}
