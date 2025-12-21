using Fluxify.Application;
using Fluxify.Persistance;

var builder = WebApplication.CreateBuilder(args);
//await builder.Services.AddRabbitMqAsync();
builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddPersistance();
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

app.MapControllers();

app.Run();
