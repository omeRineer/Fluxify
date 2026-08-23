using Fluxify.Application.Models.Enums;
using Fluxify.Application.Models.Hub;
using Fluxify.Application.Utilities.Helpers;
using Fluxify.Domain.Enums;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application.Hubs
{
    public class HubConnectionManager<THubUser>
        where THubUser : HubUser, new()
    {
        Dictionary<string, THubUser> Connections { get; }
        public HubConnectionManager()
        {
            Connections = new();
        }

        public THubUser GetConnection(Guid userId)
        {
            lock (Connections)
            {
                return Connections.Values.Single(f => f.Id == userId);
            }
        }
        public List<THubUser> GetAllConnections(Guid nameSpace)
        {
            lock (Connections)
            {
                return Connections.Values.Where(f => f.NameSpace.Id == nameSpace).ToList();
            }
        }

        public THubUser AddConnection(HubCallerContext context)
        {
            lock (Connections)
            {
                var httpContext = context.GetHttpContext();
                if (!Connections.TryGetValue(context.UserIdentifier, out THubUser user))
                {
                    user = new THubUser
                    {
                        Id = Guid.Parse(context.User.Claims.Single(f => f.Type == ClaimTypes.NameIdentifier).Value),
                        Key = context.User.Claims.Single(f => f.Type == FluxifyClaims.Key).Value,
                        NameSpace = new HubUser.NameSpaceInfo
                        {
                            Id = Guid.Parse(context.User.Claims.Single(f => f.Type == FluxifyClaims.NameSpaceId).Value),
                            Signature = context.User.Claims.Single(f => f.Type == FluxifyClaims.NameSpace).Value
                        }
                    };

                    Connections[context.UserIdentifier] = user;
                }

                user.Connections.Add(context.ConnectionId,
                                     new HubUser.DeviceInfo(httpContext?.Request?.Query[FluxifyHeaders.Device.DeviceId],
                                                            httpContext?.Request?.Query[FluxifyHeaders.Device.Platform].FirstOrDefault() ?? EnumHelper.ToEnumString<DeviceType>((int)DeviceType.Web)));

                return user;
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
