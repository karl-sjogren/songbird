using LetterAvatars.AspNetCore.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Songbird.Web.Extensions;

public static partial class StartupExtensions {
    public static void AddAvatars(this IServiceCollection services) {
        services
            .AddAvatarService()
            .AddAvatarFont()
            .AddAvatarPalette()
            .AddAvatarGenerators();
    }
}
