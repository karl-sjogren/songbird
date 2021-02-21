using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Songbird.Web.Extensions {
    public static partial class StartupExtensions {
        public static void AddEntityFramework(this IServiceCollection services, IConfiguration configuration) {
            services.AddDbContext<SongbirdContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Songbird"),
                    sqlServerOptions =>
                        sqlServerOptions
                            .CommandTimeout(120)
                    )
                    .ConfigureWarnings(warnings => warnings.Ignore(CoreEventId.ContextInitialized))
                    .EnableSensitiveDataLogging(configuration["IsLocal"] == bool.TrueString)
                );
        }
    }
}
