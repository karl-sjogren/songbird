using Microsoft.Extensions.DependencyInjection;
using RobotsTxt;

namespace Songbird.Web.Extensions;

public static partial class StartupExtensions {
    public static void AddRobotsTxt(this IServiceCollection services) {
        services
            .AddStaticRobotsTxt(builder =>
                builder
                    .DenyAll());
    }
}
