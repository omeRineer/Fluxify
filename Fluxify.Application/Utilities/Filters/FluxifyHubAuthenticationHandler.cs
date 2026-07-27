using Fluxify.Application.Models.Enums;
using Fluxify.Application.Repositories;
using Fluxify.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace Fluxify.Api.Hub.Auth
{
    public class FluxifyHubAuthenticationOptions : AuthenticationSchemeOptions
    {
    }
    public class FluxifyHubAuthenticationHandler : AuthenticationHandler<FluxifyHubAuthenticationOptions>
    {
        const string ApiKeyHeaderName = "Fluxify-Api-Key";
        readonly INameSpaceRepository NameSpaceRepository;
        readonly IUserTopicRepository UserTopicRepository;
        readonly IUserRepository UserRepository;
        public FluxifyHubAuthenticationHandler(IOptionsMonitor<FluxifyHubAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, INameSpaceRepository nameSpaceRepository, IUserTopicRepository userTopicRepository, IUserRepository userRepository) : base(options, logger, encoder, clock)
        {
            NameSpaceRepository = nameSpaceRepository;
            UserTopicRepository = userTopicRepository;
            UserRepository = userRepository;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var headers) &&
                !Request.Query.TryGetValue(ApiKeyHeaderName, out var queries))
                return AuthenticateResult.Fail("API Key is not provided.");

            var providedApiKey = headers.FirstOrDefault() ?? queries.FirstOrDefault();
            var nameSpace = await NameSpaceRepository.GetSingleAsync(f => f.ApiKey == providedApiKey);

            if (nameSpace == null)
                return AuthenticateResult.Fail("Invalid API Key.");

            if (nameSpace.ExpireDate != null && nameSpace.ExpireDate < DateTime.Now)
                return AuthenticateResult.Fail("API Key is expired.");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, nameSpace.Id.ToString()),
                new Claim(FluxifyClaims.Name, nameSpace.Name),
                new Claim(FluxifyClaims.Signature, nameSpace.Signature)

            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
