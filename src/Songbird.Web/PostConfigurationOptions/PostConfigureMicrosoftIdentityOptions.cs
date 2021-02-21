using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Songbird.Web.Contracts;

namespace Songbird.Web.PostConfigurationOptions {
    public class PostConfigureMicrosoftIdentityOptions : IPostConfigureOptions<MicrosoftIdentityOptions> {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<PostConfigureMicrosoftIdentityOptions> _logger;

        public PostConfigureMicrosoftIdentityOptions(IServiceScopeFactory serviceScopeFactory, IHttpContextAccessor httpContextAccessor, ILogger<PostConfigureMicrosoftIdentityOptions> logger) {
            _serviceScopeFactory = serviceScopeFactory;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public void PostConfigure(string name, MicrosoftIdentityOptions options) {
            _ = options ?? throw new ArgumentNullException(nameof(options));

            options.Events.OnTokenValidated = TokenValidatedAsync;
        }

        private async Task TokenValidatedAsync(TokenValidatedContext context) {
            var identity = context.Principal.Identity as ClaimsIdentity;
            var claims = identity.Claims.ToArray();

            var externalId = claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
            if(externalId == null) {
                var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                _logger.LogWarning($"User {email} was logged in through Microsoft Identity but no externalId was provided. Request will be aborted.");
                var httpContext = _httpContextAccessor.HttpContext;
                httpContext.Abort();
                return;
            }

            using var scope = _serviceScopeFactory.CreateScope();
            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            var user = await userService.CreateOrUpdateUserAsync(identity, CancellationToken.None);
            await userService.UpdateLastLoggedInAsync(user.Id, CancellationToken.None);
        }
    }
}
