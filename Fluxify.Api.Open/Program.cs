using Fluxify.Application;
using Fluxify.Configuration;
using Fluxify.Infrastructure;
using Fluxify.Persistance;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);
await builder.Services.AddRabbitMqAsync();
builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddPersistance();
builder.Services.AddCors(options =>
                            options.AddDefaultPolicy(builder =>
                            builder.WithOrigins(AppConfiguration.AllowOrigins)
                                   .AllowAnyHeader()
                                   .AllowAnyMethod()));



builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fluxify.Api.Open", Version = "v1" });
    c.CustomSchemaIds(s => s.FullName);

    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "API eriþimi için geçerli anahtarý buraya girin.",
        In = ParameterLocation.Header,
        Name = "Fluxify-Api-Key",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "FluxifySecurityScheme"
    });

    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("ApiKey", document),
            new List<string>()
        }
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.Map("/health", () => "Healthy!");
app.MapControllers();

app.Run();
