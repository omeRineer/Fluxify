using Fluxify.Application;
using Fluxify.Persistance;
using Fluxify.Application.Hubs;
using System.Security.Claims;
using Fluxify.Infrastructure;
using Fluxify.Configuration;
using Fluxify.Api.Hub.Workers.Notification;

var builder = WebApplication.CreateBuilder(args);
await builder.Services.AddRabbitMqAsync();
builder.Services.AddHostedService<NotificationWorker>();
builder.Services.AddHostedService<FirebaseNotificationWorker>();
builder.Services.AddApplication();
builder.Services.AddMongoContext();
builder.Services.AddCoreContext();
builder.Services.AddInfrastructure();
builder.Services.AddSignalR();
builder.Services.AddSingleton<HubConnectionManager<PresenceHub>>();
builder.Services.AddSingleton<HubConnectionManager<ChatHub>>();
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
