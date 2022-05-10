using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nest;
using Songbird.Web.Contracts;
using Songbird.Web.DataObjects;
using Songbird.Web.Queries;
using Songbird.Web.QueryTranslators;

namespace Songbird.Web.Services;

public class LogGraphService : ILogGraphService {
    private readonly IElasticClient _elasticClient;
    private readonly ILogger<LogSearchService> _logger;

    private static string _indexName => "logevents";

    public LogGraphService(IElasticClient elasticClient, ILogger<LogSearchService> logger) {
        _elasticClient = elasticClient;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<LogGraphEntry>> GetResultsAsync(LogGraphFilter filter, CancellationToken cancellationToken) {
        var searchDescriptor = Log4StashGraphQueryTranslator.Translate(filter);

        searchDescriptor = searchDescriptor
            .Index(_indexName);

        var result = await _elasticClient.SearchAsync<Log4StashEntry>(searchDescriptor, cancellationToken);

        var aggregation = result
            .Aggregations
            .Where(x => x.Key == "Timestamp")
            .Select(x => x.Value)
            .Cast<BucketAggregate>()
            .FirstOrDefault();

        if(aggregation == null) {
            throw new InvalidOperationException("No aggregation found in result.");
        }

        var items = aggregation
            .Items
            .Cast<DateHistogramBucket>()
            .Select(x => new LogGraphEntry {
                Label = x.Date.ToLocalTime().ToString("yyyy-MM-dd HH:mm"),
                Count = (Int32)x.DocCount
            })
            .ToList();

        return items.AsReadOnly();
    }
}
