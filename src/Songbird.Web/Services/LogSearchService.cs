using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Nest;
using Songbird.Web.Contracts;
using Songbird.Web.DataObjects;
using Songbird.Web.Exceptions;
using Songbird.Web.Extensions;
using Songbird.Web.Queries;
using Songbird.Web.QueryTranslators;

namespace Songbird.Web.Services {
    public class LogSearchService : ILogSearchService {
        private readonly IElasticClient _elasticClient;
        private readonly ILogger<LogSearchService> _logger;

        private static string _indexName => "logevents";

        public LogSearchService(IElasticClient elasticClient, ILogger<LogSearchService> logger) {
            _elasticClient = elasticClient;
            _logger = logger;
        }

        public virtual async Task<PagedResult<Log4StashEntry>> GetResultsAsync(LogSearchQuery query, LogSearchFilter filter, LogSearchSettings settings, CancellationToken cancellationToken) {
            var searchDescriptor = Log4StashQueryTranslator.Translate(query, filter, settings);

            searchDescriptor = searchDescriptor
                .Index(_indexName)
                .TrackTotalHits(true); // Lets keep exact count here

            var items = new List<Log4StashEntry>();
            long totalHits;
            if(settings.PageSize < 10_000) {
                var result = await _elasticClient.SearchAsync<Log4StashEntry>(searchDescriptor, cancellationToken);

                if(!result.IsValid)
                    throw new ElasticsearchException(result?.DebugInformation);

                items = result.Hits.Select(hit => hit.Source).ToList();
                totalHits = (Int32)result.Total;
            } else {
                _logger.LogInformation("Large search request detected, falling back to Scroll API for search.");

                // We are requesting a ton of rows so we should use the Scroll API to get everything
                searchDescriptor = searchDescriptor.Scroll("30s");

                var result = await _elasticClient.SearchAsync<Log4StashEntry>(searchDescriptor, cancellationToken);

                if(!result.IsValid)
                    throw new ElasticsearchException(result?.DebugInformation);

                items.AddRange(result.Documents);
                var scrollId = result.ScrollId;
                totalHits = result.Total;

                var scrollCounter = 1;
                do {
                    _logger.LogInformation($"Got page number {scrollCounter++} for scrollId {scrollId}.");
                    var scrollResults = await _elasticClient.ScrollAsync<Log4StashEntry>("10s", scrollId, ct: cancellationToken);
                    if(!scrollResults.IsValid)
                        throw new ElasticsearchException(scrollResults?.DebugInformation);

                    items.AddRange(scrollResults.Documents);
                    scrollId = scrollResults.ScrollId;

                    if(scrollResults.Documents.None())
                        scrollId = null;

                    if(items.Count >= settings.PageSize) {
                        scrollId = null;
                        items.RemoveRange(settings.PageSize, items.Count - settings.PageSize);
                    }

                } while(!string.IsNullOrEmpty(scrollId));
            }

            return new PagedResult<Log4StashEntry>(items, settings.PageIndex, settings.PageSize, (Int32)totalHits);
        }

        public virtual async Task<ICollection<Facet>> GetFacetsAsync(LogSearchQuery query, LogSearchFilter filter, LogSearchSettings settings, CancellationToken cancellationToken) {
            var searchDescriptor = Log4StashQueryTranslator.Translate(query, filter, settings);

            searchDescriptor = searchDescriptor
                .Index(_indexName)
                .Take(0); // No results needed for facets

            var result = await _elasticClient.SearchAsync<Log4StashEntry>(searchDescriptor, cancellationToken);
            if(!result.IsValid)
                throw new ElasticsearchException(result?.DebugInformation);

            var termFacetNames = new[] { "Application", "Level" };
            var termFacets = result.Aggregations
                .Where(agg => termFacetNames.Any(termFacet => termFacet == agg.Key))
                .Select(
                    agg => new Facet(agg.Key, "terms", ((BucketAggregate)agg.Value)
                        .Items
                        .Cast<KeyedBucket<object>>()
                        .Select(bucket => new FacetValue(bucket.Key?.ToString(), bucket.Key?.ToString(), (Int32)bucket.DocCount))
                        .ToList()
                )
            ).ToList();

            var dateFacetNames = new[] { "Timestamp" };
            var dateFacets = result.Aggregations
                .Where(agg => dateFacetNames.Any(dateFacet => dateFacet == agg.Key))
                .Select(
                    agg => new Facet(agg.Key, "date-histogram", ((BucketAggregate)agg.Value)
                        .Items
                        .Cast<DateHistogramBucket>()
                        .Select(bucket => new FacetValue(bucket.Date.ToString("yyyy-MM-dd"), bucket.Date.ToString("yyyy-MM-dd"), (Int32)bucket.DocCount))
                        .ToList()
                )
            ).ToList();

            var facets = termFacets.Concat(dateFacets).ToList();

            var missingFacets = termFacetNames.Concat(dateFacetNames).Where(facetName => !facets.Any(facet => facet.Name == facetName));
            foreach(var facetName in missingFacets) {
                facets.Add(new Facet(facetName, "terms", Array.Empty<FacetValue>()));
            }

            return facets;
        }

        public async Task<Log4StashEntry> GetEntryAsync(string identifier, CancellationToken cancellationToken) {
            var result = await _elasticClient.GetAsync<Log4StashEntry>(identifier, ct: cancellationToken);
            if(!result.IsValid)
                throw new ElasticsearchException(result?.DebugInformation);

            return result.Source;
        }
    }
}
