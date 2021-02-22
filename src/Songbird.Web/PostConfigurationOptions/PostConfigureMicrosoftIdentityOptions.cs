using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using Songbird.Web.Contracts;
using Songbird.Web.Models;

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

            using var serviceScope = _serviceScopeFactory.CreateScope();

            var userService = serviceScope.ServiceProvider.GetRequiredService<IUserService>();
            var user = await userService.CreateOrUpdateUserAsync(identity, CancellationToken.None);
            await userService.UpdateLastLoggedInAsync(user.Id, CancellationToken.None);
            await UpdateUserPhotoAsync(serviceScope, user.Id, context.Principal);
        }

        private static async Task UpdateUserPhotoAsync(IServiceScope serviceScope, Guid userId, ClaimsPrincipal claimsPrincipal) {
            var songbirdContext = serviceScope.ServiceProvider.GetRequiredService<SongbirdContext>();
            var dateTimeProvider = serviceScope.ServiceProvider.GetRequiredService<IDateTimeProvider>();
            var tokenAcquisition = serviceScope.ServiceProvider.GetRequiredService<ITokenAcquisition>();

            var token = await tokenAcquisition.GetAccessTokenForUserAsync(new[] { "user.read" }, user: claimsPrincipal);

            var graphService = new GraphServiceClient(new DelegateAuthenticationProvider(
                    requestMessage => {
                        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                        return Task.CompletedTask;
                    }));

            var graphPhoto = await graphService.Me.Photo.Request().GetAsync(CancellationToken.None);

            graphPhoto.AdditionalData.TryGetValue("@odata.mediaContentType", out var contentTypeObject);
            var contentType = (string)contentTypeObject ?? "image/jpeg";

            graphPhoto.AdditionalData.TryGetValue("@odata.mediaEtag", out var etagObject);
            var etag = (string)etagObject ?? string.Empty;

            var photo = await songbirdContext.UserPhotos.FirstOrDefaultAsync(x => x.UserId == userId);
            if(string.IsNullOrEmpty(etag) && photo != null) {
                songbirdContext.UserPhotos.Remove(photo);
                await songbirdContext.SaveChangesAsync();
                return;
            }

            if(photo?.ETag == etag) {
                return;
            }

            if(photo == null) {
                photo = new UserPhoto {
                    UserId = userId,
                    CreatedAt = dateTimeProvider.Now
                };
                await songbirdContext.AddAsync(photo);
            }

            using var photoStream = await graphService.Me.Photo.Content.Request().GetAsync(CancellationToken.None);

            photo.Content = ((MemoryStream)photoStream).ToArray();
            photo.ContentType = contentType;
            photo.ETag = etag;
            photo.UpdatedAt = dateTimeProvider.Now;

            await songbirdContext.SaveChangesAsync();
        }
    }
}
