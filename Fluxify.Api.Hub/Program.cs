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
using Fluxify.Domain.Enums;

var builder = WebApplication.CreateBuilder(args);
await builder.Services.AddRabbitMqAsync();
builder.Services.AddHostedService<PushSocketNotificationWorker>();
builder.Services.AddHostedService<PushFirebaseNotificationWorker>();
builder.Services.AddHostedService<SendMessageWorker>();
builder.Services.AddApplication();
builder.Services.AddTokenAuthentication(new JwtBearerEvents
{
    OnMessageReceived = (context) =>
    {
        if (!context.Request.Query.TryGetValue("access_token", out var queries))
            return Task.CompletedTask;

        var token = queries.FirstOrDefault();

        if (!string.IsNullOrEmpty(token))
            context.Token = token;

        return Task.CompletedTask;
    },
    OnTokenValidated = (context) =>
    {
        var deviceId =
        context.Request.Query[FluxifyHeaders.Device.DeviceId].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(deviceId))
        {
            context.Fail($"{FluxifyHeaders.Device.DeviceId} is required.");
            return Task.CompletedTask;
        }

        var platform =
            context.Request.Query[FluxifyHeaders.Device.Platform].FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(platform) &&
            !Enum.TryParse<DeviceType>(platform, true, out _))
        {
            context.Fail($"Invalid {FluxifyHeaders.Device.Platform}.");
            return Task.CompletedTask;
        }

        return Task.CompletedTask;
    }
});
builder.Services.AddMongoContext();
builder.Services.AddCoreContext();
builder.Services.AddInfrastructure();
builder.Services.AddSignalR();
builder.Services.AddSingleton<HubConnectionManager<PresenceHubUser>>();
builder.Services.AddSingleton<HubConnectionManager<ChatHubUser>>();
builder.Services.AddSingleton<HubConnectionManager<NotificationHubUser>>();
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
app.MapHub<NotificationHub>("/hub/notification");

app.Run();
