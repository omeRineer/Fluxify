using Fluxify.Application;
using Fluxify.Application.Models.Common;
using Fluxify.Application.Models.Enums;
using Fluxify.Configuration;
using Fluxify.Infrastructure;
using Fluxify.Persistance;
using Microsoft.OpenApi;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

await builder.Services.AddRabbitMqAsync();
builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddGlobalExceptionHandler();
builder.Services.AddApiKeyAuthentication();
builder.Services.AddSessionContext<CurrentNameSpace>(httpContext =>
{
    var user = httpContext?.User;
    var userClaims = user.Claims
        .GroupBy(k => k.Type)
        .ToDictionary(g => g.Key, g => g.First().Value);


    if (user != null && user.Identity.IsAuthenticated)
        return new CurrentNameSpace
        {
            NameIdentifier = Guid.Parse(userClaims[ClaimTypes.NameIdentifier]),
            Name = userClaims[FluxifyClaims.Name],
            Signature = userClaims[FluxifyClaims.Signature]
        };

    return new CurrentNameSpace();
});
builder.Services.AddMongoContext();
builder.Services.AddCoreContext();
builder.Services.AddCors(options =>
                            options.AddDefaultPolicy(builder =>
                            builder.AllowAnyOrigin()
                                   .AllowAnyHeader()
                                   .AllowAnyMethod()));
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fluxify.Api.Integration", Version = "v1" });
    c.CustomSchemaIds(s => s.FullName?.Replace('+', '_'));

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

app.UseSwagger(opt => { opt.RouteTemplate = "integrationapi/swagger/{documentName}/swagger.json"; });
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = "integrationapi/swagger";
    c.SwaggerEndpoint("/integrationapi/swagger/v1/swagger.json", "Fluxify Integration API v1");
});

app.UseExceptionHandler();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();