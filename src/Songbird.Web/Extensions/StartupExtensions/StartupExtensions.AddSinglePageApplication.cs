using Microsoft.Extensions.DependencyInjection;

namespace Songbird.Web.Extensions {
    public static partial class StartupExtensions {
        public static void AddSinglePageApplication(this IServiceCollection services) {
            // In production, the Ember files will be served from this directory
            services.AddSpaStaticFiles(configuration => configuration.RootPath = "ClientApp");
        }
    }
}
