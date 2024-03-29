using System;
using System.Linq;
using Nest;
using Songbird.Web.DataObjects;
using Songbird.Web.Queries;

namespace Songbird.Web.QueryTranslators;

public static class Log4StashQueryTranslator {
    public static SearchDescriptor<Log4StashEntry> Translate(LogSearchQuery query, LogSearchFilter filter, LogSearchSettings settings) {
        var descriptor = new SearchDescriptor<Log4StashEntry>();

        var filters = BuildFilter(filter);

        var container = new QueryContainer();
        var builder = new QueryContainerDescriptor<Log4StashEntry>();
        container &= BuildQuery(query);
        container &= builder.Bool(b => b
            .Filter(filters));

        descriptor
            .Query(_ => container);

        ApplySettings(descriptor, settings);
        AddAggregations(descriptor);

        return descriptor;
    }

    private static QueryContainer BuildQuery(LogSearchQuery query) {
        var container = new QueryContainer();
        var builder = new QueryContainerDescriptor<Log4StashEntry>();

        if(query.IsEmpty) {
            container &= builder.MatchAll();
            return container;
        }

        if(!string.IsNullOrWhiteSpace(query.Query)) {
            var fields = new[] {
                "Message^2",
                "LoggerName",
                "HostName",
                "Exception",
                "ExceptionObject.Message",
                "ExceptionObject.StackTrace",
                "ExceptionObject.InnerException.Message",
                "ExceptionObject.InnerException.StackTrace",
                "ExceptionObject.InnerException.InnerException.Message",
                "ExceptionObject.InnerException.InnerException.StackTrace",
                "ExceptionObject.InnerException.InnerException.InnerException.Message",
                "ExceptionObject.InnerException.InnerException.InnerException.StackTrace"
            };

            container &= Query<Log4StashEntry>
                .MultiMatch(m => m
                    .Query(query.Query)
                    .Type(TextQueryType.CrossFields)
                    .Operator(Operator.And)
                    .Fields(fields));
        }

        return container;
    }

    private static QueryContainer BuildFilter(LogSearchFilter filter) {
        var container = new QueryContainer();
        var builder = new QueryContainerDescriptor<Log4StashEntry>();

        if(filter.Application?.Any() == true) {
            var subContainer = new QueryContainer();
            foreach(var value in filter.Application)
                subContainer |= builder.Term("log4net:UserName.keyword", value);
            container &= subContainer;
        }

        if(filter.DateRange?.Any() == true) {
            var subContainer = new QueryContainer();
            foreach(var value in filter.DateRange)
                subContainer |= builder.DateRange(r => r.Field("@timestamp").GreaterThanOrEquals(value.MinDate).LessThanOrEquals(value.MaxDate));
            container &= subContainer;
        }

        if(filter.Level?.Any() == true) {
            var subContainer = new QueryContainer();
            foreach(var value in filter.Level)
                subContainer |= builder.Term("Level.keyword", value);
            container &= subContainer;
        }

        return container;
    }

    private static void ApplySettings(SearchDescriptor<Log4StashEntry> descriptor, LogSearchSettings settings) {
        descriptor = descriptor
            .Skip(settings.PageIndex * settings.PageSize)
            .Take(settings.PageSize > 10_000 ? 10_000 : settings.PageSize);

        var sortField = settings.SortField;
        if(string.IsNullOrWhiteSpace(sortField)) {
            sortField = "@timestamp";
        }

        if(settings.SortAscending)
            descriptor = descriptor.Sort(s => s.Ascending(sortField));
        else
            descriptor = descriptor.Sort(s => s.Descending(sortField));
    }

    private static void AddAggregations(SearchDescriptor<Log4StashEntry> descriptor) {
#pragma warning disable CS0618 // Needed to use the "Obsolete" method .Interval since this is to be used against an older version of Elastiscsearch
        descriptor.Aggregations(aggregation => aggregation
            .Terms("Application", f => f.Field("log4net:UserName.keyword").Size(100))
            .Terms("Level", f => f.Field("Level.keyword").Size(100))
            .DateHistogram("Timestamp", f => f.Field("@timestamp").Interval(new Time(TimeSpan.FromDays(1)))));
#pragma warning restore CS0618
    }
}
