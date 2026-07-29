using Fluxify.Application;
using Fluxify.Application.Models.Common;
using Fluxify.Application.Models.Enums;
using Fluxify.Configuration;
using Fluxify.Domain.Enums;
using Fluxify.Infrastructure;
using Fluxify.Persistance;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
            Name = userClaims[FluxifyClaims.Name]
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




var app = builder.Build();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
