using Fluxify.Dashboard.Components;
using Fluxify.Dashboard.Providers;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<HubConnectionProvider>();
builder.Services.AddRadzenComponents();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
