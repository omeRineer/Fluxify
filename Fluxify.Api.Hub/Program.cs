using Fluxify.Application;
using Fluxify.Persistance;
using Fluxify.Application.Hubs;
using System.Security.Claims;
using Fluxify.Api.Hub.Workers;
using Fluxify.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
await builder.Services.AddRabbitMqAsync();
builder.Services.AddHostedService<NotificationWorker>();
builder.Services.AddHostedService<FirebaseNotificationWorker>();
builder.Services.AddApplication();
builder.Services.AddPersistance();
builder.Services.AddInfrastructure();
builder.Services.AddSignalR();
builder.Services.AddSingleton<HubConnectionManager<NotificationHub>>();
builder.Services.AddSingleton<HubConnectionManager<PresenceHub>>();
builder.Services.AddCors(options =>
                            options.AddDefaultPolicy(policy =>
                            {
                                policy.WithOrigins("http://127.0.0.1:5500", "http://localhost:5500", "https://localhost:3000") // Client'ýnýzýn çalýþtýðý adresleri virgülle ekleyin
                                      .AllowAnyHeader()
                                      .AllowAnyMethod()
                                      .AllowCredentials();
                            }));


var app = builder.Build();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapHub<NotificationHub>("/hub/notification");
app.MapHub<PresenceHub>("/hub/presence");

app.Run();
