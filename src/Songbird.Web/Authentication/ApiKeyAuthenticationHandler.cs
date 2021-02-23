using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Songbird.Web.Authentication {
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<ApiKeyAuthenticationOptions> {
        private const string _problemDetailsContentType = "application/problem+json";
        private const string _apiKeyHeaderName = "X-Api-Key";

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<ApiKeyAuthenticationOptions> options,
            UrlEncoder encoder,
            ISystemClock clock,
            ILoggerFactory logger) : base(options, logger, encoder, clock) {
        }

#pragma warning disable CS1998
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {
#pragma warning restore CS1998
            string providedApiKey;
            if(Request.Headers.TryGetValue(_apiKeyHeaderName, out var apiKeyHeaderValues))
                providedApiKey = apiKeyHeaderValues.FirstOrDefault();
            else
                providedApiKey = Request.Query["apikey"].FirstOrDefault();

            if(string.IsNullOrWhiteSpace(providedApiKey))
                return AuthenticateResult.NoResult();

            if(providedApiKey != "fake-api-key") // TODO read this from config
                return AuthenticateResult.Fail("Invalid API Key provided.");

            var claims = new List<Claim> {
                new Claim(ClaimTypes.Name, "API User")
            };

            var identity = new ClaimsIdentity(claims, Options.AuthenticationType);
            var identities = new List<ClaimsIdentity> { identity };
            var principal = new ClaimsPrincipal(identities);
            var ticket = new AuthenticationTicket(principal, Options.Scheme);

            return AuthenticateResult.Success(ticket);
        }

        protected override Task HandleChallengeAsync(AuthenticationProperties properties) {
            Response.StatusCode = 401;
            Response.ContentType = _problemDetailsContentType;

            return Task.CompletedTask;
        }

        protected override Task HandleForbiddenAsync(AuthenticationProperties properties) {
            Response.StatusCode = 403;
            Response.ContentType = _problemDetailsContentType;

            return Task.CompletedTask;
        }
    }
}
