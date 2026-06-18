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
        readonly IUserService NotificationSubscriberService;
        readonly INameSpaceRepository NameSpaceRepository;
        readonly ITopicService topicService;
        public FluxifyHubAuthenticationHandler(IOptionsMonitor<FluxifyHubAuthenticationOptions> options, IUserService notificationSubscriberService, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, INameSpaceRepository nameSpaceRepository, ITopicService topicService) : base(options, logger, encoder, clock)
        {
            NotificationSubscriberService = notificationSubscriberService;
            NameSpaceRepository = nameSpaceRepository;
            this.topicService = topicService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue(ApiKeyHeaderName, out var headers) &
                !Request.Query.TryGetValue(ApiKeyHeaderName, out var queries))
                return AuthenticateResult.Fail("API Key is not provided.");

            var providedApiKey = headers.FirstOrDefault() ?? queries.FirstOrDefault();
            var subscriber = (await NotificationSubscriberService.GetAsync(providedApiKey)).Data;

            if (subscriber == null)
                return AuthenticateResult.Fail("Invalid API Key.");

            if (subscriber.ExpireDate != null && subscriber.ExpireDate < DateTime.Now)
                return AuthenticateResult.Fail("API Key is expired.");

            var nameSpace = await NameSpaceRepository.GetSingleAsync(f=> f.Id == subscriber.NameSpace);

            var claims = new List<Claim>
            {
                new Claim(FluxifyClaims.Key, subscriber.Key),
                new Claim(ClaimTypes.NameIdentifier, subscriber.Id.ToString()),
                new Claim(FluxifyClaims.ApiKey, subscriber.ApiKey),
                new Claim(FluxifyClaims.NameSpaceId, subscriber.NameSpace.ToString()),
                new Claim(FluxifyClaims.NameSpace, nameSpace.Signature),
                new Claim(FluxifyClaims.Device, "undefined"),
                new Claim(FluxifyClaims.Platform, "undefined"),
                new Claim(ClaimTypes.Name, subscriber.Name)
            };

            var userTopicsResult = await topicService.GetListAsync(subscriber.Id);
            if (userTopicsResult.Data != null && userTopicsResult.Data.Count > 0)
                foreach (var topic in userTopicsResult.Data)
                    claims.Add(new Claim(FluxifyClaims.Topic, topic.Key));

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
    }
}
