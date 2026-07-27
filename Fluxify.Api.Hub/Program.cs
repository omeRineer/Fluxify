using Fluxify.Application;
using Fluxify.Persistance;
using Fluxify.Application.Hubs;
using System.Security.Claims;
using Fluxify.Infrastructure;
using Fluxify.Configuration;
using Fluxify.Api.Hub.Workers.Notification;
using Fluxify.Api.Hub.Workers.Chat;
using Fluxify.Application.Models.Hub;
using Fluxify.Application.Models.Common;
using Fluxify.Application.Models.Enums;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);
await builder.Services.AddRabbitMqAsync();
builder.Services.AddHostedService<PushSocketNotificationWorker>();
builder.Services.AddHostedService<PushFirebaseNotificationWorker>();
builder.Services.AddHostedService<SendMessageWorker>();
builder.Services.AddApplication();
builder.Services.AddTokenAuthentication(new JwtBearerEvents
{
    OnMessageReceived = context =>
    {
        if (!context.Request.Query.TryGetValue("access_token", out var queries))
            return Task.CompletedTask;

        var token = queries.FirstOrDefault();

        if (!string.IsNullOrEmpty(token))
            context.Token = token;

        return Task.CompletedTask;
    }
});
builder.Services.AddSessionContext<CurrentUser>(httpContext =>
{
    var user = httpContext?.User;
    var userClaims = user.Claims
        .GroupBy(k => k.Type)
        .ToDictionary(g => g.Key, g => g.First().Value);


    if (user != null && user.Identity.IsAuthenticated)
        return new CurrentUser
        {
            NameIdentifier = Guid.Parse(userClaims[ClaimTypes.NameIdentifier]),
            Key = userClaims[FluxifyClaims.Key],
            NameSpace = new CurrentUser.NameSpaceInfo(Guid.Parse(userClaims[FluxifyClaims.NameSpaceId]),
                                                      userClaims[FluxifyClaims.NameSpace]),
            Name = userClaims[ClaimTypes.Name],
            Device = new CurrentUser.DeviceInfo(userClaims[FluxifyClaims.Device], userClaims[FluxifyClaims.Platform])
        };

    return new CurrentUser();
});
builder.Services.AddMongoContext();
builder.Services.AddCoreContext();
builder.Services.AddInfrastructure();
builder.Services.AddSignalR();
builder.Services.AddSingleton<HubConnectionManager<PresenceHubUser>>();
builder.Services.AddSingleton<HubConnectionManager<ChatHubUser>>();
builder.Services.AddCors(options =>
                            options.AddDefaultPolicy(policy =>
                            {
                                policy.WithOrigins(AppConfiguration.AllowOrigins)
                                      .AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .AllowCredentials();
                            }));


var app = builder.Build();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapHub<PresenceHub>("/hub/presence");
app.MapHub<ChatHub>("/hub/chat");

app.Run();
