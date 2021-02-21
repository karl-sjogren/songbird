using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Web;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Songbird.Web.Extensions;
using Songbird.Web.PostConfigurationOptions;
using Microsoft.Extensions.Options;

namespace Songbird.Web.Extensions {
    public static partial class StartupExtensions {
        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration) {
            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(configuration.GetSection("AzureAd"));

            services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<MicrosoftIdentityOptions>, PostConfigureMicrosoftIdentityOptions>());
        }
    }
}
