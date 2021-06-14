using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Songbird.Web.DataObjects;
using Songbird.Web.Queries;

namespace Songbird.Web.Contracts {
    public interface ILogSearchService {
        public Task<PagedResult<Log4StashEntry>> GetResultsAsync(LogSearchQuery query, LogSearchFilter filter, LogSearchSettings settings, CancellationToken cancellationToken);
        Task<ICollection<Facet>> GetFacetsAsync(LogSearchQuery query, LogSearchFilter filter, LogSearchSettings settings, CancellationToken cancellationToken);
        public Task<Log4StashEntry> GetEntryAsync(string identifier, CancellationToken cancellationToken);
    }
}
