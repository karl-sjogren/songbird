using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Songbird.Web.Extensions {
    public static partial class StartupExtensions {
        public static void AddRobotsTxt(this IServiceCollection services) {
            services
                .AddStaticRobotsTxt(builder =>
                    builder
                        .DenyAll());
        }
    }
}
