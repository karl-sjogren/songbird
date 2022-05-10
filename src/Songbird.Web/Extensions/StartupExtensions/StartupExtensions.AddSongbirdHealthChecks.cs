using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Songbird.Web.Extensions;

public static partial class StartupExtensions {
    public static void AddSongbirdHealthChecks(this IServiceCollection services) {
        services
            .AddHealthChecks()
            .AddDbContextCheck<SongbirdContext>(name: "SQL Database", customTestQuery: async (context, cancellationToken) => {
                var connection = context.Database.GetDbConnection();
                try {
                    await connection.OpenAsync(cancellationToken);
                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT 1 AS [DISABLE_EF_LOGGING]";

                    await command.ExecuteNonQueryAsync(cancellationToken);
                } catch {
                    return false;
                } finally {
                    await connection.CloseAsync();
                }

                return true;
            });
    }
}
