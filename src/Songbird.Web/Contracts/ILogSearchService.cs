using System.Threading;
using System.Threading.Tasks;
using Songbird.Web.Queries;

namespace Songbird.Web.Contracts {
    public interface ILogSearchService {
        public Task SearchAsync(LogSearchQuery query, LogSearchFilter filter, LogSearchSettings settings, CancellationToken cancellationToken);
        public Task GetItemAsync(string identifier, CancellationToken cancellationToken);
    }
}
