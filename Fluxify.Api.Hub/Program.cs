using Fluxify.Application;
using Fluxify.Persistance;
using Fluxify.Application.Hubs;
using Fluxify.Application.Models.Client;
using System.Security.Claims;
using Fluxify.Api.Hub.Workers;

var builder = WebApplication.CreateBuilder(args);
await builder.Services.AddRabbitMqAsync();
builder.Services.AddHostedService<NotificationWorker>();
builder.Services.AddApplication();
builder.Services.AddPersistance();
builder.Services.AddSignalR();
builder.Services.AddSingleton<HubConnectionManager<NotificationHub>>();
builder.Services.AddSingleton<HubConnectionManager<PresenceHub>>();
builder.Services.AddCors(options =>
                            options.AddDefaultPolicy(builder =>
                            builder.AllowAnyHeader()
                                   .AllowAnyMethod()
                                   .AllowAnyOrigin()));


var app = builder.Build();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapHub<NotificationHub>("/hub/notification");
app.MapHub<PresenceHub>("/hub/presence");

app.Run();
