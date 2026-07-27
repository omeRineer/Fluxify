using Fluxify.Application;
using Fluxify.Application.Models.Common;
using Fluxify.Application.Models.Enums;
using Fluxify.Configuration;
using Fluxify.Infrastructure;
using Fluxify.Persistance;
using Fluxify.Persistance.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
await builder.Services.AddRabbitMqAsync();
builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddTokenAuthentication();
builder.Services.AddSessionContext<CurrentUser>(httpContext =>
{
    var user = httpContext?.User;
    var userClaims = user.Claims
        .GroupBy(k => k.Type)
        .ToDictionary(g => g.Key, g => g.First().Value);


    if (user != null && user.Identity.IsAuthenticated)
        return new CurrentUser
        {
            NameIdentifier = Guid.Parse(userClaims[ClaimTypes.NameIdentifier]),
            Key = userClaims[FluxifyClaims.Key],
            NameSpace = new CurrentUser.NameSpaceInfo(Guid.Parse(userClaims[FluxifyClaims.NameSpaceId]),
                                                      userClaims[FluxifyClaims.NameSpace]),
            Name = userClaims[ClaimTypes.Name],
            Device = new CurrentUser.DeviceInfo(userClaims[FluxifyClaims.Device], userClaims[FluxifyClaims.Platform])
        };

    return new CurrentUser();
});
builder.Services.AddMongoContext();
builder.Services.AddCoreContext();
builder.Services.AddCors(options =>
                            options.AddDefaultPolicy(builder =>
                            builder.WithOrigins(AppConfiguration.AllowOrigins)
                                   .AllowAnyHeader()
                                   .AllowAnyMethod()));



builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fluxify.Api.Open", Version = "v1" });
    c.CustomSchemaIds(s => s.FullName?.Replace('+', '_'));

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Yöneticinizden aldýðýnýz tokeni giriniz",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", document),
            new List<string>()
        }
    });
});





var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CoreContext>();
    dbContext.Database.Migrate();
}

app.UseSwagger(opt=> { opt.RouteTemplate = "openapi/swagger/{documentName}/swagger.json"; });
app.UseSwaggerUI(c =>
{
    c.RoutePrefix = "openapi/swagger";
    c.SwaggerEndpoint("/openapi/swagger/v1/swagger.json", "Fluxify Open API v1");
});

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
