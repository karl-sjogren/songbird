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
                .AddControllers(options => options.ModelBinderProviders.RemoveType<DateTimeModelBinderProvider>())
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
