using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;

namespace Songbird.Web.Extensions;

public static partial class StartupExtensions {
    public static void AddControllersWithSerialization(this IServiceCollection services) {
        services
            .AddHttpContextAccessor()
            .AddControllers(options => {
                options.ModelBinderProviders.RemoveType<DateTimeModelBinderProvider>();
                options.CacheProfiles.Add("Image",
                    new CacheProfile() {
                        Duration = (Int32)TimeSpan.FromDays(1).TotalSeconds,
                        Location = ResponseCacheLocation.Any
                    });
            })
            .AddJsonOptions(options => {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.WriteIndented = true;
                //options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });
    }
}
