using System;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Graph;
using Microsoft.Identity.Web;
using Songbird.Web.Contracts;
using Songbird.Web.Models;

namespace Songbird.Web.PostConfigurationOptions;

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

        var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        var externalId = claims.FirstOrDefault(c => c.Type == "http://schemas.microsoft.com/identity/claims/objectidentifier")?.Value;
        if(externalId == null) {
            _logger.LogWarning("User {email} was logged in through Microsoft Identity but no externalId was provided. Request will be aborted.", email);
            var httpContext = _httpContextAccessor.HttpContext;
            httpContext.Abort();
            return;
        }

        if(email?.EndsWith("@xlent.se", true, null) != true) {
            _logger.LogWarning("User {email} was logged in through Microsoft Identit but the email wasn't a valid XLENT email. Request will be aborted.", email);
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
        var configuration = serviceScope.ServiceProvider.GetRequiredService<IConfiguration>();
        var dateTimeProvider = serviceScope.ServiceProvider.GetRequiredService<IDateTimeProvider>();
        var tokenAcquisition = serviceScope.ServiceProvider.GetRequiredService<ITokenAcquisition>();
        var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<PostConfigureMicrosoftIdentityOptions>>();

        var scopes = configuration.GetValue<string>("GraphApi:Scopes")?.Split(' ', StringSplitOptions.TrimEntries);
        var token = await tokenAcquisition.GetAccessTokenForUserAsync(scopes, user: claimsPrincipal);

        var graphService = new GraphServiceClient(new DelegateAuthenticationProvider(
                requestMessage => {
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    return Task.CompletedTask;
                }));

        ProfilePhoto profilePhoto;
        try {
            profilePhoto = await graphService.Me.Photo.Request().GetAsync(CancellationToken.None);
        } catch(ServiceException ex) when(ex.StatusCode == System.Net.HttpStatusCode.NotFound) {
            logger.LogInformation("User {userId} has no photo in Microsoft Graph. Photo will not be updated.", userId);
            return;
        }

        profilePhoto.AdditionalData.TryGetValue("@odata.mediaContentType", out var contentTypeObject);
        var contentTypeElement = (JsonElement)contentTypeObject;
        var contentType = contentTypeElement.GetString() ?? "image/jpeg";

        profilePhoto.AdditionalData.TryGetValue("@odata.mediaEtag", out var etagObject);
        var etagElement = (JsonElement)etagObject;
        var etag = etagElement.GetString() ?? string.Empty;

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

        await songbirdContext.SaveChangesAsync(CancellationToken.None);
    }
}
