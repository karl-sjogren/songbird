using System;
using Microsoft.AspNetCore.Authentication;
using Songbird.Web.Authentication;

namespace Songbird.Web.Extensions;

public static class AuthenticationBuilderExtensions {
    public static AuthenticationBuilder AddApiKeys(this AuthenticationBuilder authenticationBuilder, Action<ApiKeyAuthenticationOptions> options = null) {
        if(options == null)
            options = _ => { };

        return authenticationBuilder.AddScheme<ApiKeyAuthenticationOptions, ApiKeyAuthenticationHandler>(ApiKeyAuthenticationOptions.AuthenticationScheme, options);
    }
}
