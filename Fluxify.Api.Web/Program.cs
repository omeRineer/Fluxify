using Fluxify.Application;
using Fluxify.Configuration;
using Fluxify.Infrastructure;
using Fluxify.Persistance;

var builder = WebApplication.CreateBuilder(args);

await builder.Services.AddRabbitMqAsync();
builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddCurrentUserInfo();
builder.Services.AddTokenAuthentication();
builder.Services.AddMongoContext();
builder.Services.AddCoreContext();
builder.Services.AddCors(options =>
                            options.AddDefaultPolicy(builder =>
                            builder.WithOrigins(AppConfiguration.AllowOrigins)
                                   .AllowAnyHeader()
                                   .AllowAnyMethod()));

var app = builder.Build();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
