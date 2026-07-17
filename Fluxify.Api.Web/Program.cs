using Fluxify.Application;
using Fluxify.Persistance;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddMongoContext();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
