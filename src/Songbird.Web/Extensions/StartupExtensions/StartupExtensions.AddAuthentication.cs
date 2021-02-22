using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Songbird.Web.Extensions;
using Songbird.Web.PostConfigurationOptions;

namespace Songbird.Web.Extensions {
    public static partial class StartupExtensions {
        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration) {
            var initialScopes = configuration.GetValue<string>("GraphApi:Scopes")?.Split(' ');

            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(configuration.GetSection("AzureAd"))
                .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
                .AddMicrosoftGraph(configuration.GetSection("GraphApi"))
                .AddDistributedTokenCaches();

            services.AddDistributedSqlServerCache(options => {
                options.ConnectionString = configuration["ConnectionStrings:Songbird"];
                options.SchemaName = "dbo";
                options.TableName = "DistributedCache";
            });

            services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<MicrosoftIdentityOptions>, PostConfigureMicrosoftIdentityOptions>());
        }
    }
}
