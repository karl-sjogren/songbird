using System;
using System.Linq;
using Nest;
using Songbird.Web.DataObjects;
using Songbird.Web.Queries;

namespace Songbird.Web.QueryTranslators {
    public static class Log4StashGraphQueryTranslator {
        public static SearchDescriptor<Log4StashEntry> Translate(LogGraphFilter filter) {
            var descriptor = new SearchDescriptor<Log4StashEntry>();

            var filters = BuildFilter(filter);

            var container = new QueryContainer();
            var builder = new QueryContainerDescriptor<Log4StashEntry>();
            container &= builder.Bool(b => b
                .Filter(filters));

            descriptor
                .Take(0)
                .Query(_ => container);

            AddAggregations(descriptor, filter);

            return descriptor;
        }

        private static QueryContainer BuildFilter(LogGraphFilter filter) {
            var container = new QueryContainer();
            var builder = new QueryContainerDescriptor<Log4StashEntry>();

            if(filter.Application?.Any() == true) {
                var subContainer = new QueryContainer();
                foreach(var value in filter.Application)
                    subContainer |= builder.Term("log4net:UserName.keyword", value);
                container &= subContainer;
            }

            if(filter.Level?.Any() == true) {
                var subContainer = new QueryContainer();
                foreach(var value in filter.Level)
                    subContainer |= builder.Term("Level.keyword", value);
                container &= subContainer;
            }

            container &= builder.DateRange(r => r.Field("@timestamp").GreaterThanOrEquals(filter.DateRange.MinDate).LessThanOrEquals(filter.DateRange.MaxDate));

            return container;
        }

        private static void AddAggregations(SearchDescriptor<Log4StashEntry> descriptor, LogGraphFilter filter) {
#pragma warning disable CS0618 // Needed to use the "Obsolete" method .Interval since this is to be used against an older version of Elastiscsearch
            descriptor.Aggregations(aggregation => aggregation
                .DateHistogram("Timestamp", f => f
                    .Field("@timestamp")
                    .MinimumDocumentCount(0)
                    .Interval(new Time(TimeSpan.FromHours(3)))
                    .ExtendedBounds(filter.DateRange.MinDate, filter.DateRange.MaxDate)
                    .TimeZone("+" + TimeZoneInfo.Local.GetUtcOffset(DateTime.Today).ToString("hh\\:mm"))));
#pragma warning restore CS0618
        }
    }
}
