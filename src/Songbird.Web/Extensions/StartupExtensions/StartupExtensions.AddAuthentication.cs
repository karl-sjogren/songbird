using System.Linq;
using AcadeSongbirdMedia.Web.Extensions;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Web;
using Songbird.Web.Authentication;
using Songbird.Web.Extensions;
using Songbird.Web.PostConfigurationOptions;

namespace Songbird.Web.Extensions {
    public static partial class StartupExtensions {
        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration) {
            var initialScopes = configuration.GetValue<string>("GraphApi:Scopes")?.Split(' ');

            services.AddAuthentication(x => {
                x.DefaultScheme = "DynamicScheme";
                x.DefaultSignInScheme = OpenIdConnectDefaults.AuthenticationScheme;
                x.DefaultSignOutScheme = OpenIdConnectDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            }).AddPolicyScheme("DynamicScheme", "Authorization Bearer or Saml2", options => {
                options.ForwardDefaultSelector = context => {
                    var apiKeyHeader = context.Request.Headers["X-API-Key"].FirstOrDefault();
                    var apiKeyQuery = context.Request.Query["apikey"].FirstOrDefault();

                    if(apiKeyHeader != null || apiKeyQuery != null)
                        return ApiKeyAuthenticationOptions.AuthenticationScheme;

                    return OpenIdConnectDefaults.AuthenticationScheme;
                };
            })
                .AddApiKeys()
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
