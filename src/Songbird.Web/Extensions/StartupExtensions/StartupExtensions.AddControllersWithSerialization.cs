using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Songbird.Web.Extensions {
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
                .AddNewtonsoftJson(o => {
                    o.SerializerSettings.ContractResolver = new DefaultContractResolver {
                        NamingStrategy = new CamelCaseNamingStrategy {
                            ProcessDictionaryKeys = false
                        }
                    };
                    o.SerializerSettings.Converters.Add(new StringEnumConverter());
                    o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
        }
    }
}
