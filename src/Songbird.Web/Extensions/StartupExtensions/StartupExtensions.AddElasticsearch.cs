using System;
using Elasticsearch.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Nest;
using Nest.JsonNetSerializer;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Songbird.Web.Options;

namespace Songbird.Web.Extensions;

public static partial class StartupExtensions {
    public static void AddElasticsearch(this IServiceCollection services) {
        services.AddSingleton<IElasticClient>(provider => {
            var optionsAccessor = provider.GetService<IOptions<ElasticsearchOptions>>();
            var options = optionsAccessor.Value;
            var elasticEndpointUri = new Uri(options.Endpoint, UriKind.Absolute);
            var connectionPool = new SingleNodeConnectionPool(elasticEndpointUri);

            var settings = new ConnectionSettings(
                connectionPool,
                sourceSerializer: (builtin, settings) => new JsonNetSerializer(builtin, settings, () => {
                    var settings = new JsonSerializerSettings {
                        NullValueHandling = NullValueHandling.Ignore,
                        ContractResolver = new DefaultContractResolver {
                            NamingStrategy = new CamelCaseNamingStrategy {
                                ProcessDictionaryKeys = false
                            }
                        }
                    };
                    settings.Converters.Add(new StringEnumConverter());
                    settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    return settings;
                }))
                .EnableDebugMode()
                .EnableTcpStats(false)
                .ThrowExceptions(false);

            if(!string.IsNullOrWhiteSpace(options.Username)) {
                settings.BasicAuthentication(options.Username, options.Password);
            }

            return new ElasticClient(settings);
        });
    }
}
