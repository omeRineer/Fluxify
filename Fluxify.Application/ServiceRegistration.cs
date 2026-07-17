using AutoMapper;
using AutoMapper.Internal;
using Fluxify.Api.Hub.Auth;
using Fluxify.Application.Models.Common;
using Fluxify.Application.Models.Enums;
using Fluxify.Application.Repositories;
using Fluxify.Application.Services.Core;
using Fluxify.Application.Services.Interfaces.Core;
using Fluxify.Application.Services.Interfaces.Mongo;
using Fluxify.Application.Services.Mongo;
using Fluxify.Application.Utilities.Helpers;
using Fluxify.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Fluxify.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplication(this IServiceCollection services)
        {
            //services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<INameSpaceService, NameSpaceService>();
            services.AddScoped<IUserService, UserService>();
            //services.AddScoped<IUserNotificationService, UserNotificationService>();
            services.AddScoped<IUserDeviceService, UserDeviceService>();
            services.AddScoped<IUserTopicService, UserTopicService>();
            services.AddScoped<IChannelService, ChannelService>();
            services.AddScoped<ITopicService, TopicService>();

            services.AddHttpContextAccessor();
            services.AddAuthentication()
                .AddScheme<FluxifyHubAuthenticationOptions, FluxifyHubAuthenticationHandler>("FluxifySecurityScheme", null);


            services.AddScoped<CurrentUser>(i =>
            {
                var httpContextAccessor = i.GetService<IHttpContextAccessor>();
                var user = httpContextAccessor?.HttpContext?.User;
                var userClaims = user.Claims
                    .GroupBy(k => k.Type)
                    .ToDictionary(g => g.Key, g => g.First().Value);


                if (user != null && user.Identity.IsAuthenticated)
                    return new CurrentUser
                    {
                        NameIdentifier = Guid.Parse(userClaims[ClaimTypes.NameIdentifier]),
                        ApiKey = userClaims[FluxifyClaims.ApiKey],
                        Key = userClaims[FluxifyClaims.Key],
                        NameSpace = new CurrentUser.NameSpaceInfo(Guid.Parse(userClaims[FluxifyClaims.NameSpaceId]),
                                                                  userClaims[FluxifyClaims.NameSpace]),
                        Name = userClaims[ClaimTypes.Name],
                        Device = new CurrentUser.DeviceInfo(userClaims[FluxifyClaims.Device], userClaims[FluxifyClaims.Platform])
                    };

                return new CurrentUser();
            });

            services.AddAutoMapper(opt =>
            {
                
            }, Assembly.GetExecutingAssembly());
        }


    }
}
