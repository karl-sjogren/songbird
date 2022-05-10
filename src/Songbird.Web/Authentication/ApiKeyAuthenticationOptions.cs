using Microsoft.AspNetCore.Authentication;

namespace Songbird.Web.Authentication;

public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions {
    public const string AuthenticationScheme = "API Key";
    public string Scheme => AuthenticationScheme;
    public string AuthenticationType = AuthenticationScheme;
}
